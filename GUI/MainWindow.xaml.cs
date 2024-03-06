using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace GUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly MainWindowViewmodel _vm = new();
    private readonly SerialPort _port;

    private HandToRcConverter _handToRcConverter = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
            
        DispatcherTimer timer = new DispatcherTimer();
        timer.Tick += TickEventHandler;
        timer.Interval = TimeSpan.FromMilliseconds(20);
        timer.Start();

        HandTracker.initialize_connection();
        HandTracker.start_polling();

        _port = new SerialPort
        {
            PortName = "COM6",
            Handshake = Handshake.None,
            DtrEnable = true
        };
        _port.Open();
            
    }

    private void WriteValuesToPort(IEnumerable<byte> rcValues)
    {
        if (!_port.IsOpen)
        {
            return;
        }

        var toWrite = new List<byte>{0xff}.Concat(rcValues);
            
        // var packet= ((byte[]) [0xff, 100, 100, 100, 100, 60, 60, 60, 60, 60]).AsMemory(0, 10);
        _port.Write(toWrite.ToArray(), 0, 10);
    }
        
    ~MainWindow()
    {
        HandTracker.stop_polling();
        HandTracker.close_connection();
        _port.Close();
    }

    private void TickEventHandler(object? sender, EventArgs e)
    {
        HandTracker.GetHandsData().Deconstruct(out var leftHand, out var rightHand);

        // var leftGrabPercent = leftGrab * 50 + 50;
        // var leftPinchPercent = leftPinch * 50 + 50;
        //
        // _vm.RawRightRoll = rollPercent;
        // _vm.RawRightYaw = yawPercent;
        // _vm.RawRightPitch = pitchPercent;
        // _vm.RawLeftGrab = leftGrabPercent;
        // _vm.RawLeftPitch = leftPinchPercent;
        // _vm.RawLeftX = leftX;
        // _vm.RawLeftY = throttlePercent;
        // _vm.RawLeftZ = leftZ;
        //
        //
        // _rcValues.pitch = (byte) Math.Round(pitchPercent, 0);
        // _rcValues.roll = (byte) Math.Round(rollPercent, 0);
        // _rcValues.yaw = (byte) Math.Round(rollPercent, 0);
        // _rcValues.throttle = (byte)Math.Round(throttlePercent, 0);
        _handToRcConverter.LeftHand = leftHand;
        _handToRcConverter.RightHand = rightHand;
        _vm.RcValues = _handToRcConverter.GetRcValues;
        
        _vm.LeftHandValid = HandTracker.LeftHandCount switch
        {
            0 => HandValid.NoHands,
            1 => HandValid.Valid,
            _ => HandValid.TooManyHands
        };

        _vm.RightHandValid = HandTracker.RightHandCount switch
        {
            0 => HandValid.NoHands,
            1 => HandValid.Valid,
            _ => HandValid.TooManyHands
        };

        if (HandTracker.LeftHandCount + HandTracker.RightHandCount == 2)
        {
            WriteValuesToPort(_handToRcConverter.GetRcValues.AsList);
        }
    }

    private void OpenRawInputWindow_ButtonOnClick(object sender, RoutedEventArgs e)
    {
        var window = new RawInputWindow();
        window.Show();
    }

    private void OpenCalibrationWindow_ButtonOnClick(object sender, RoutedEventArgs e)
    {
        var window = new CalibrationWindow(_handToRcConverter.Calibrations);
        window.ShowDialog();
        _handToRcConverter.Calibrations = window.Calibrations;
    }
}