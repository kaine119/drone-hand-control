using System.Runtime.InteropServices;

namespace GUI;

internal static class HandTracker
{
    [DllImport("HandTracker.dll")]
    internal static extern bool initialize_connection();
    
    [DllImport("HandTracker.dll")]
    internal static extern bool close_connection();
    
    [DllImport("HandTracker.dll")]
    internal static extern bool start_polling();
    
    [DllImport("HandTracker.dll")]
    internal static extern bool stop_polling();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_roll();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_pitch();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_right_hand_yaw();
 
    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_grab();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_pinch();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_x();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_y();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_left_hand_z();
}