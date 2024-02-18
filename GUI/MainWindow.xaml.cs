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
        }
    }
}
