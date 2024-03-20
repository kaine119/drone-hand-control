namespace GUI;

public class HandToRcConverter
{
    public Hand LeftHand = new();
    public Hand RightHand = new();
    public HandTrackerAxisCalibrations Calibrations = new();

    public RCValues GetRcValues =>
        new()
        {
            Roll = Calibrations.Roll.InterpolateValues(RightHand.Roll), // Roll
            Pitch = Calibrations.Pitch.InterpolateValues(RightHand.Pitch), // Pitch
            Throttle = Calibrations.Throttle.InterpolateValues(-LeftHand.Z), // Throttle
            Yaw = Calibrations.Yaw.InterpolateValues(LeftHand.Roll),
            Arm = LeftHand.GrabStrength > 0.7 ? 100 : 0,
            Mode = 100,
            Kill = 0,
            Cam = RightHand.GrabStrength > 0.7 ? 100 : 0,
            Grab = RightHand.PinchStrength > 0.7 ? 100 : 0
        };
}