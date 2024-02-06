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
    internal static extern float get_roll();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_pitch();
    
    [DllImport("HandTracker.dll")]
    internal static extern float get_yaw();
    
}