using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI;

public record AxisCalibration : INotifyPropertyChanged
{
    public float Zero = 0;
    public double Min = -1;
    public double Max = 1;

    private bool _applyExpo = false;
    private float _expo = 1.5f;
    private float _deadband = 0f;

    public float Expo
    {
        get => _expo;
        set
        {
            if (value.Equals(_expo)) return;
            _expo = value;
            OnPropertyChanged();
        }
    }

    public bool ApplyExpo
    {
        get => _applyExpo;
        set
        {
            if (value == _applyExpo) return;
            _applyExpo = value;
            OnPropertyChanged();
        }
    }

    public float Deadband
    {
        get => _deadband;
        set
        {
            if (value.Equals(_deadband)) return;
            _deadband = value;
            OnPropertyChanged();
        }
    }

    public int InterpolateValues(float value)
    {
        if (value > Max) return 100;
        if (value < Min) return 0;
        return ApplyExpo ? _expoInterpolate(value) : _linearInterpolate(value);
    }

    private int _expoInterpolate(float value)
    {
        if (Expo == 0)
        {
            return _linearInterpolate(value);
        }

        if (value - Zero > 0)
        {
            var initialFraction = (value - Zero) / (Max - Zero); // within [0, 1]
            if (initialFraction < Deadband)
            {
                return 50;
            }

            var deadbandZero = Deadband * (Max - Zero) + Zero;

            var fraction = (value - deadbandZero) / (Max - deadbandZero); // within [0, 1]
            var scaleFactor = 50 / (Math.Exp(Expo) - 1);
            return 50 + (int)Math.Round(scaleFactor * (Math.Exp(Expo * fraction) - 1), 0);
        }
        else
        {
            var initialFraction = (value - Zero) / (Min - Zero); // within [0, 1]
            if (initialFraction < Deadband)
            {
                return 50;
            }

            var deadbandZero = Deadband * (Min - Zero) + Zero;

            var fraction = (value - deadbandZero) / (Min - deadbandZero); // within [0, 1]
            var scaleFactor = 50 / (Math.Exp(Expo) - 1);
            return 50 - (int)Math.Round(scaleFactor * (Math.Exp(Expo * fraction) - 1), 0);
        }
    }

    private int _linearInterpolate(float value)
    {
        if (value - Zero > 0)
        {
            var initialFraction = (value - Zero) / (Max - Zero); // within [0, 1]
            if (initialFraction < Deadband)
            {
                return 50;
            }

            var deadbandZero = Deadband * (Max - Zero) + Zero;
            
            var fraction = (value - deadbandZero) / (Max - deadbandZero); // within [0, 1]
            return 50 + (int)Math.Round(fraction * 50, 0);
        }
        else
        {
            var initialFraction = (value - Zero) / (Min - Zero); // within [0, 1]
            if (initialFraction < Deadband)
            {
                return 50;
            }

            var deadbandZero = Deadband * (Min - Zero) + Zero;

            var fraction = (value - deadbandZero) / (Min - deadbandZero); // within [0, 1]
            return 50 - (int)Math.Round(fraction * 50, 0);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public record HandTrackerAxisCalibrations : INotifyPropertyChanged
{
    private AxisCalibration _roll = new();

    private AxisCalibration _pitch = new();

    private AxisCalibration _throttle = new()
    {
        Zero = 0,
        Min = -100,
        Max = 100
    };

    private AxisCalibration _yaw = new();

    public AxisCalibration Roll
    {
        get => _roll;
        set
        {
            if (Equals(value, _roll)) return;
            _roll = value;
            OnPropertyChanged();
        }
    }

    public AxisCalibration Pitch
    {
        get => _pitch;
        set
        {
            if (Equals(value, _pitch)) return;
            _pitch = value;
            OnPropertyChanged();
        }
    }

    public AxisCalibration Throttle
    {
        get => _throttle;
        set
        {
            if (Equals(value, _throttle)) return;
            _throttle = value;
            OnPropertyChanged();
        }
    }

    public AxisCalibration Yaw
    {
        get => _yaw;
        set
        {
            if (Equals(value, _yaw)) return;
            _yaw = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}