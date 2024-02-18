using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GUI;

record Hand
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

internal static class HandTracker
{
    #region DLL imports - connection management

    [DllImport("HandTracker.dll")]
    internal static extern bool initialize_connection();


    [DllImport("HandTracker.dll")]
    internal static extern bool close_connection();

    [DllImport("HandTracker.dll")]
    internal static extern bool start_polling();

    [DllImport("HandTracker.dll")]
    internal static extern bool stop_polling();

    #endregion

    #region DLL imports - getters

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_x();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_y();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_z();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_roll();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_pitch();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_yaw();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_grab();

    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_pinch();


    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_x();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_y();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_z();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_roll();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_pitch();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_yaw();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_grab();

    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_pinch();

    #endregion

    public static Tuple<Hand, Hand> GetHandsData()
    {
        var leftHand = new Hand
        {
            Type = Hand.Chirality.Left,
            X = get_left_hand_x(),
            Y = get_left_hand_y(),
            Z = get_left_hand_z(),
            Roll = get_left_hand_roll(),
            Pitch = get_left_hand_pitch(),
            Yaw = get_left_hand_yaw(),
            GrabStrength = get_left_hand_grab(),
            PinchStrength = get_left_hand_pinch()
        };

        var rightHand = new Hand
        {
            Type = Hand.Chirality.Right,
            X = get_right_hand_x(),
            Y = get_right_hand_y(),
            Z = get_right_hand_z(),
            Roll = get_right_hand_roll(),
            Pitch = get_right_hand_pitch(),
            Yaw = get_right_hand_yaw(),
            GrabStrength = get_right_hand_grab(),
            PinchStrength = get_right_hand_pinch()
        };

        return new Tuple<Hand, Hand>(leftHand, rightHand);
    }
}