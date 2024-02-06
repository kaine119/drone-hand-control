#include <iostream>
#include <windows.h>
#include "LeapC.h"

static float hand_roll = 0;
static float hand_pitch = 0;
static float hand_yaw = 0;

static bool is_thread_running = false;

static LEAP_CONNECTION connection;
static HANDLE thread;
static DWORD thread_handle;


static DWORD WINAPI run_thread(LPVOID params)
{
    while (is_thread_running) {
        const unsigned int timeout = 300000;
        LEAP_CONNECTION_MESSAGE message;
        const eLeapRS result = LeapPollConnection(connection, timeout, &message);
        if (result != eLeapRS_Success) {
            std::cout << "Something went wrong!" << std::endl;
            return 1;
        }
        if (message.device_id == 0 || message.tracking_event->nHands == 0) {
            continue;
        }
        const LEAP_VECTOR hand_vector = message.tracking_event->pHands->palm.normal;
        hand_roll = hand_vector.x;
        hand_pitch = hand_vector.z;
        hand_yaw = hand_vector.y;
    }
    return 0;
}

extern "C" {
    __declspec(dllexport) BOOL __cdecl initialize_connection ();
    __declspec(dllexport) BOOL __cdecl close_connection ();
    __declspec(dllexport) void __cdecl start_polling ();
    __declspec(dllexport) void __cdecl stop_polling ();
    __declspec(dllexport) float __cdecl get_roll ();
    __declspec(dllexport) float __cdecl get_pitch ();
    __declspec(dllexport) float __cdecl get_yaw ();
}

FLOAT get_roll() { return hand_roll; }
FLOAT get_pitch() { return hand_pitch; }
FLOAT get_yaw() { return hand_yaw; }

BOOL initialize_connection()
{
    eLeapRS result = LeapCreateConnection(nullptr, &connection);
    if (result != eLeapRS_Success) {
        std::cout << "Something went wrong!" << std::endl;
        return false;
    }

    result = LeapOpenConnection(connection);
    if (result != eLeapRS_Success) {
        std::cout << "Something went wrong!" << std::endl;
        return false;
    }

    return true;
}

BOOL close_connection()
{
    LeapCloseConnection(connection);
    return true;
}

void start_polling()
{
    is_thread_running = true;
    thread = CreateThread(NULL, 0, run_thread, NULL, 0, &thread_handle);
}

void stop_polling()
{
    is_thread_running = false;
    WaitForSingleObject(thread, INFINITE);
    thread = nullptr;
}
