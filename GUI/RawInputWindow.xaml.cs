using System;
using System.Windows;
using System.Windows.Threading;

namespace GUI;

public partial class RawInputWindow : Window
{
    private readonly RawInputWindowViewmodel _vm = new();

    public RawInputWindow()
    {
        InitializeComponent();
        DataContext = _vm;

        DispatcherTimer timer = new DispatcherTimer();
        timer.Tick += TickEventHandler;
        timer.Interval = TimeSpan.FromMilliseconds(20);
        timer.Start();
    }

    private void TickEventHandler(object? sender, EventArgs e)
    {
        HandTracker.GetHandsData().Deconstruct(out var leftHand, out var rightHand);

        _vm.LeftHandValid = HandTracker.LeftHandCount switch
        {
            0 => HandValid.NoHands,
            1 => HandValid.Valid,
            _ => HandValid.TooManyHands
        };

        _vm.RightHandValid = HandTracker.LeftHandCount switch
        {
            0 => HandValid.NoHands,
            1 => HandValid.Valid,
            _ => HandValid.TooManyHands
        };

        _vm.LeftHand = leftHand;
        _vm.RightHand = rightHand;
    }
}