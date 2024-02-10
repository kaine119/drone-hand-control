using System;
using System.Windows;
using System.Windows.Threading;


namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewmodel _vm;
        
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
        }

        ~MainWindow()
        {
            HandTracker.stop_polling();
            HandTracker.close_connection();
        }

        private void TickEventHandler(object? sender, EventArgs e)
        {
            var pitch = HandTracker.get_right_hand_pitch();
            var roll = HandTracker.get_right_hand_roll();
            var yaw = HandTracker.get_right_hand_yaw();
            var leftGrab = HandTracker.get_left_hand_grab();
            var leftPinch = HandTracker.get_left_hand_pinch();
            var leftX = HandTracker.get_left_hand_x();
            var leftY = HandTracker.get_left_hand_y();
            var leftZ = HandTracker.get_left_hand_z();

            _vm.HandRoll = roll * 50 + 50;
            _vm.HandYaw = yaw * 50 + 50;
            _vm.HandPitch = pitch * 50 + 50;
            _vm.LeftHandGrab = leftGrab * 50 + 50;
            _vm.LeftHandPinch = leftPinch * 50 + 50;
            _vm.LeftHandX = leftX;
            _vm.LeftHandY = leftY;
            _vm.LeftHandZ = leftZ;
        }
    }
}
