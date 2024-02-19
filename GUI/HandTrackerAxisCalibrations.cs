using System;

namespace GUI;

public record AxisCalibration
{
    public float Zero = 0;
    public double Min = -1;
    public double Max = 1;
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