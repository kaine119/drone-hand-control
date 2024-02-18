using System.Collections.Generic;

class RCValues
{
    public byte roll;
    public byte pitch;
    public byte throttle;
    public byte yaw;
    public byte arm;
    public byte grab;
    private byte cam;
    private byte kill;
    private byte mode;

    public byte Roll
    {
        get => roll;
        set => roll = value > 100 ? (byte)100 : value;
    }

    public byte Pitch
    {
        get => pitch;
        set => pitch = value > 100 ? (byte)100 : value;
    }

    public byte Throttle
    {
        get => throttle;
        set => throttle = value > 100 ? (byte)100 : value;
    }

    public byte Yaw
    {
        get => yaw;
        set => yaw = value > 100 ? (byte)100 : value;
    }

    public byte Arm
    {
        get => arm;
        set => arm = value > 100 ? (byte)100 : value;
    }

    public byte Grab
    {
        get => grab;
        set => grab = value > 100 ? (byte)100 : value;
    }
    
    public byte Cam
    {
        get => cam;
        set => cam = value > 100 ? (byte)100 : value;
    }

    public byte Kill
    {
        get => kill;
        set => kill = value > 100 ? (byte)100 : value;
    }

    public byte Mode
    {
        get => mode;
        set => mode = value > 100 ? (byte)100 : value;
    }

    public List<byte> AsList => [roll, pitch, throttle, pitch, arm, mode, kill, cam, grab];
}