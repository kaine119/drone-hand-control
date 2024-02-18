using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using System.IO.Ports;
using System.Linq;

class RCValues
{
    public byte roll;
    public byte pitch;
    public byte throttle;
    public byte yaw;
    public byte arm;
    public byte grab;
    private byte cam;
    private byte kill;
    private byte mode;

    public byte Roll
    {
        get => roll;
        set => roll = value > 100 ? (byte)100 : value;
    }

    public byte Pitch
    {
        get => pitch;
        set => pitch = value > 100 ? (byte)100 : value;
    }

    public byte Throttle
    {
        get => throttle;
        set => throttle = value > 100 ? (byte)100 : value;
    }

    public byte Yaw
    {
        get => yaw;
        set => yaw = value > 100 ? (byte)100 : value;
    }

    public byte Arm
    {
        get => arm;
        set => arm = value > 100 ? (byte)100 : value;
    }

    public byte Grab
    {
        get => grab;
        set => grab = value > 100 ? (byte)100 : value;
    }
    
    public byte Cam
    {
        get => cam;
        set => cam = value > 100 ? (byte)100 : value;
    }

    public byte Kill
    {
        get => kill;
        set => kill = value > 100 ? (byte)100 : value;
    }

    public byte Mode
    {
        get => mode;
        set => mode = value > 100 ? (byte)100 : value;
    }

    public List<byte> AsList => [roll, pitch, throttle, pitch, arm, mode, kill, cam, grab];
}

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewmodel _vm;
        private readonly SerialPort _port;

        private RCValues _rcValues = new();

        public MainWindow()
        {
            _vm = new MainWindowViewmodel();
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

        private void WriteValuesToPort()
        {
            Debug.WriteLine("Port open? {0}", _port.IsOpen);
            if (!_port.IsOpen)
            {
                Debug.WriteLine("port was not open!");
                return;
            }

            var toWrite = new List<byte>{0xff}.Concat(_rcValues.AsList);
            
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
        }
    }
}
