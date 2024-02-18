namespace GUI;

public record Hand
{
    public enum Chirality
    {
        Left,
        Right
    }

    public Chirality Type;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float Roll { get; set; }
    public float Pitch { get; set; }
    public float Yaw { get; set; }
    public float GrabStrength { get; set; }
    public float PinchStrength { get; set; }
}