using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI;

public class CalibrationWindowViewmodel : INotifyPropertyChanged
{
    private HandTrackerAxisCalibrations _calibrations = new();
    private HandToRcConverter _handToRcConverter = new();
    private string _instructions = "";


    public HandTrackerAxisCalibrations Calibrations
    {
        get => _calibrations;
        set
        {
            if (Equals(value, _calibrations)) return;
            _calibrations = value;
            OnPropertyChanged();
        }
    }

    public string Instructions
    {
        get => _instructions;
        set
        {
            if (value == _instructions) return;
            _instructions = value;
            OnPropertyChanged();
        }
    }

    public HandToRcConverter HandToRcConverter
    {
        get => _handToRcConverter;
        set
        {
            if (Equals(value, _handToRcConverter)) return;
            _handToRcConverter = value;
            OnPropertyChanged();
        }
    }

    public RCValues RcValues { get; set; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}