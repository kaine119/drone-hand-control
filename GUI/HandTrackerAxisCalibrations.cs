using System;

namespace GUI;

public record AxisCalibration
{
    public float Zero = 0;
    public double Min = -1;
    public double Max = 1;

    public int InterpolateValues(float value)
    {
        // TODO: change to expo
        if (value - Zero > 0)
            return (int)Math.Round(50 + (value - Zero) / (Max - Zero) * 50, 0);
        return (int)Math.Round(50 - (value - Zero) / (Min - Zero) * 50, 0);
    }
}

public record HandTrackerAxisCalibrations
{
    public AxisCalibration Roll = new();
    public AxisCalibration Pitch = new();
    public AxisCalibration Throttle = new()
    {
        Zero = 0,
        Min = -100,
        Max = 100
    };
    public AxisCalibration Yaw = new();
}