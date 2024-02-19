using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace GUI;

public enum HandValid
{
    NoHands,
    Valid,
    TooManyHands
}


[ValueConversion(typeof(HandValid), typeof(Color))]
public class ValidHandsToColorConverter : IValueConverter
{

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var handValid = (HandValid?)value;
        return handValid switch
        {
            HandValid.Valid => new SolidColorBrush(Color.FromRgb(80, 200, 80)),
            _ => new SolidColorBrush(Color.FromRgb(200, 80, 80))
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return HandValid.Valid;
    }
}


[ValueConversion(typeof(HandValid), typeof(string))]
public class ValidHandsToLabelConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var handValid = (HandValid?)value;
        return handValid switch
        {
            HandValid.Valid => "Valid",
            HandValid.NoHands => "No hands detected",
            HandValid.TooManyHands => "Too many hands detected",
            _ => "???"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return HandValid.Valid;
    }
}

public sealed class RawInputWindowViewmodel: INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private Hand? _leftHand;
    private Hand? _rightHand;
    private HandValid _leftHandValid = HandValid.NoHands;
    private HandValid _rightHandValid = HandValid.NoHands;

    public Hand? LeftHand
    {
        get => _leftHand;
        set
        {
            if (Equals(value, _leftHand)) return;
            _leftHand = value;
            OnPropertyChanged();
        }
    }

    public Hand? RightHand
    {
        get => _rightHand;
        set
        {
            if (Equals(value, _rightHand)) return;
            _rightHand = value;
            OnPropertyChanged();
        }
    }

    public HandValid LeftHandValid
    {
        get => _leftHandValid;
        set
        {
            if (value == _leftHandValid) return;
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

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}