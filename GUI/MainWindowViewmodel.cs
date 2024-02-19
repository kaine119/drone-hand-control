using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI;

public sealed class MainWindowViewmodel : INotifyPropertyChanged
{
    private HandValid _leftHandValid;
    private HandValid _rightHandValid;
    private RCValues _rcValues = new();
    public event PropertyChangedEventHandler? PropertyChanged;

    public HandValid LeftHandValid
    {
        get => _leftHandValid;
        set
        {
            if (Equals(value, _leftHandValid)) return;
            _leftHandValid = value;
            OnPropertyChanged();
        }
    }

    public HandValid RightHandValid
    {
        get => _rightHandValid;
        set
        {
            if (value == _rightHandValid) return;
            _rightHandValid = value;
            OnPropertyChanged();
        }
    }

    public RCValues RcValues
    {
        get => _rcValues;
        set
        {
            if (Equals(value, _rcValues)) return;
            _rcValues = value;
            OnPropertyChanged();
        }
    }


    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}