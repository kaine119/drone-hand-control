using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GUI;

public partial class CalibrationWindow : Window
{
    private readonly CalibrationWindowViewmodel _vm = new();
    public HandTrackerAxisCalibrations Calibrations;

    private bool _calibrationRunning = false;
    private int _currentField = 0;

    private Hand _leftHand = new();
    private Hand _rightHand = new();

    private readonly List<string> _instructions =
    [
        "Move roll to MID position and press Shift",
        "Move roll to MAX position and press Shift",
        "Move roll to MIN position and press Shift",

        "Move pitch to MID position and press Shift",
        "Move pitch to MAX position and press Shift",
        "Move pitch to MIN position and press Shift",

        "Move throttle to MID position and press Shift",
        "Move throttle to MAX position and press Shift",
        "Move throttle to MIN position and press Shift",

        "Move yaw to MID position and press Shift",
        "Move yaw to MAX position and press Shift",
        "Move yaw to MIN position and press Shift"
    ];

    public CalibrationWindow(HandTrackerAxisCalibrations calibrations)
    {
        Calibrations = calibrations;
        _vm.Calibrations = Calibrations;

        InitializeComponent();
        DataContext = _vm;

        var timer = new DispatcherTimer();
        timer.Tick += TickEventHandler;
        timer.Interval = TimeSpan.FromMilliseconds(20);
        timer.Start();

        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (!_calibrationRunning) return;
        if (e.Key is not (Key.LeftShift or Key.RightShift)) return;
        
        var axisToCalibrate = (int)Math.Floor((decimal)_currentField / 3);
        switch (axisToCalibrate)
        {
            case 0:
                // roll
                switch (_currentField % 3)
                {
                    case 0:
                        Debug.WriteLine("Setting Roll zero = {0}", _rightHand.Roll);
                        Calibrations.Roll.Zero = _rightHand.Roll;
                        break;
                    case 1:
                        Debug.WriteLine("Setting Roll max = {0}", _rightHand.Roll);
                        Calibrations.Roll.Max = _rightHand.Roll;
                        break;
                    case 2:
                        Debug.WriteLine("Setting Roll min = {0}", _rightHand.Roll);
                        Calibrations.Roll.Min = _rightHand.Roll;
                        break;
                }

                break;
            case 1:
                // pitch
                switch (_currentField % 3)
                {
                    case 0:
                        Debug.WriteLine("Setting Pitch zero = {0}", _rightHand.Pitch);
                        Calibrations.Pitch.Zero = _rightHand.Pitch;
                        break;
                    case 1:
                        Debug.WriteLine("Setting Pitch max = {0}", _rightHand.Pitch);
                        Calibrations.Pitch.Max = _rightHand.Pitch;
                        break;
                    case 2:
                        Debug.WriteLine("Setting Pitch min = {0}", _rightHand.Pitch);
                        Calibrations.Pitch.Min = _rightHand.Pitch;
                        break;
                }

                break;
            case 2:
                // throttle
                switch (_currentField % 3)
                {
                    case 0:
                        Debug.WriteLine("Setting Throttle zero = {0}", -_rightHand.Z);
                        Calibrations.Throttle.Zero = -_leftHand.Z;
                        break;
                    case 1:
                        Debug.WriteLine("Setting Throttle max = {0}", -_rightHand.Z);
                        Calibrations.Throttle.Max = -_leftHand.Z;
                        break;
                    case 2:
                        Debug.WriteLine("Setting Throttle min = {0}", -_rightHand.Z);
                        Calibrations.Throttle.Min = -_leftHand.Z;
                        break;
                }

                break;
            case 3:
                // yaw
                switch (_currentField % 3)
                {
                    case 0:
                        Debug.WriteLine("Setting Yaw zero = {0}", _leftHand.Roll);
                        Calibrations.Yaw.Zero = _leftHand.Roll;
                        break;
                    case 1:
                        Debug.WriteLine("Setting Yaw max = {0}", _leftHand.Roll);
                        Calibrations.Yaw.Max = _leftHand.Roll;
                        break;
                    case 2:
                        Debug.WriteLine("Setting Yaw min = {0}", _leftHand.Roll);
                        Calibrations.Yaw.Min = _leftHand.Roll;
                        break;
                }

                break;
        }

        if (_currentField >= 11)
        {
            _vm.Instructions = "Done! Close this window to apply calibration";
            Debug.WriteLine("Roll Zero: {0}", Calibrations.Roll.Zero);
            Debug.WriteLine("Roll Max: {0}", Calibrations.Roll.Max);
            Debug.WriteLine("Roll Min: {0}", Calibrations.Roll.Min);

            Debug.WriteLine("Pitch Zero: {0}", Calibrations.Pitch.Zero);
            Debug.WriteLine("Pitch Max: {0}", Calibrations.Pitch.Max);
            Debug.WriteLine("Pitch Min: {0}", Calibrations.Pitch.Min);

            Debug.WriteLine("Throttle Zero: {0}", Calibrations.Throttle.Zero);
            Debug.WriteLine("Throttle Max: {0}", Calibrations.Throttle.Max);
            Debug.WriteLine("Throttle Min: {0}", Calibrations.Throttle.Min);

            Debug.WriteLine("Yaw Zero: {0}", Calibrations.Yaw.Zero);
            Debug.WriteLine("Yaw Max: {0}", Calibrations.Yaw.Max);
            Debug.WriteLine("Yaw Min: {0}", Calibrations.Yaw.Min);
        }
        else
        {
            _currentField += 1;
            _vm.Instructions = _instructions[_currentField];
        }
    }

    private void TickEventHandler(object? sender, EventArgs e)
    {
        HandTracker.GetHandsData().Deconstruct(out _leftHand, out _rightHand);

        RCValues newValues = new HandToRcConverter
        {
            LeftHand = _leftHand,
            RightHand = _rightHand,
            Calibrations = Calibrations
        }.GetRcValues;

        _vm.RcValues.Roll = newValues.Roll;
        _vm.RcValues.Pitch = newValues.Pitch;
        _vm.RcValues.Throttle = newValues.Throttle;
        _vm.RcValues.Yaw = newValues.Yaw;
    }

    private void StartCalibration_ButtonOnClick(object sender, RoutedEventArgs e)
    {
        _calibrationRunning = true;
        _currentField = 0;
        _vm.Instructions = _instructions[0];
    }
}