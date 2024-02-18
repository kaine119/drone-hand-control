using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GUI;

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
    internal static extern int get_right_hand_count();
    
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
    internal static extern int get_left_hand_count();
    
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

    public static int LeftHandCount() => get_left_hand_count();
    public static int RightHandCount() => get_right_hand_count();
}