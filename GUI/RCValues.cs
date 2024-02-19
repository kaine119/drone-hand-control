using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class RCValues : INotifyPropertyChanged
{
    private int _roll = 0;
    private int _pitch = 0;
    private int _throttle = 0;
    private int _yaw = 0;
    private int _arm = 0;
    private int _grab = 0;
    private int _cam = 0;
    private int _kill = 0;
    private int _mode = 0;

    public List<byte> AsList =>
    [
        (byte)Roll, (byte)Pitch, (byte)Throttle, (byte)Yaw, (byte)Arm, (byte)Mode, (byte)Kill, (byte)Cam, (byte)Grab
    ];


    public int Roll
    {
        get => _roll;
        set
        {
            if (value == _roll) return;
            _roll = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Pitch
    {
        get => _pitch;
        set
        {
            if (value == _pitch) return;
            _pitch = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Throttle
    {
        get => _throttle;
        set
        {
            if (value == _throttle) return;
            _throttle = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Yaw
    {
        get => _yaw;
        set
        {
            if (value == _yaw) return;
            _yaw = value;
            OnPropertyChanged();
        }
    }

    public int Arm
    {
        get => _arm;
        set
        {
            if (value == _arm) return;
            _arm = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Grab
    {
        get => _grab;
        set
        {
            if (value == _grab) return;
            _grab = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Cam
    {
        get => _cam;
        set
        {
            if (value == _cam) return;
            _cam = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Kill
    {
        get => _kill;
        set
        {
            if (value == _kill) return;
            _kill = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }

    public int Mode
    {
        get => _mode;
        set
        {
            if (value == _mode) return;
            _mode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AsList));
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}