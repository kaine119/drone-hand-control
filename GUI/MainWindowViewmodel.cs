using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI;

public class MainWindowViewmodel : INotifyPropertyChanged
{
    private float _handRoll;
    private float _handPitch;
    private float _handYaw;
    public event PropertyChangedEventHandler? PropertyChanged;

    public float HandRoll
    {
        get => _handRoll;
        set
        {
            if (value.Equals(_handRoll)) return;
            _handRoll = value;
            OnPropertyChanged();
        }
    }

    public float HandPitch
    {
        get => _handPitch;
        set
        {
            if (value.Equals(_handPitch)) return;
            _handPitch = value;
            OnPropertyChanged();
        }
    }

    public float HandYaw
    {
        get => _handYaw;
        set
        {
            if (value.Equals(_handYaw)) return;
            _handYaw = value;
            OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}