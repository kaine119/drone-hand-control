using System;
using System.Collections.Generic;

namespace GUI;

public class HandToRcConverter
{
    public Hand LeftHand = new();
    public Hand RightHand = new();
    public HandTrackerAxisCalibrations Calibrations = new();

    public RCValues GetRcValues =>
        new()
        {
            Roll = InterpolateValues(RightHand.Roll, Calibrations.Roll), // Roll
            Pitch = InterpolateValues(RightHand.Pitch, Calibrations.Pitch), // Pitch
            Throttle = InterpolateValues(-LeftHand.Z, Calibrations.Throttle), // Throttle
            Yaw = InterpolateValues(LeftHand.Roll, Calibrations.Yaw),
            Arm = LeftHand.GrabStrength > 0.7 ? 100 : 0,
            Mode = 50,
            Kill = 0,
            Cam = 0,
            Grab = LeftHand.PinchStrength > 0.7 ? 100 : 0
        };

    private int InterpolateValues(float value, AxisCalibration calibration)
    {
        if (value - calibration.Zero > 0)
            return (int)Math.Round(50 + (value - calibration.Zero) / (calibration.Max - calibration.Zero) * 50, 0);
        return (int)Math.Round(50 - (value - calibration.Zero) / (calibration.Min - calibration.Zero) * 50, 0);
    }
}