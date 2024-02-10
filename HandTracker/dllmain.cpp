#include <iostream>
#include <windows.h>
#include "LeapC.h"

static float right_hand_roll = 0;
static float right_hand_pitch = 0;
static float right_hand_yaw = 0;

static float left_hand_pinch;
static float left_hand_grab;

static bool is_thread_running = false;

static LEAP_CONNECTION connection;
static HANDLE thread;
static DWORD thread_handle;


static float left_hand_x;
static float left_hand_y;
static float left_hand_z;

static DWORD WINAPI run_thread(LPVOID params)
{
    while (is_thread_running) {
        const unsigned int timeout = 300000;
        LEAP_CONNECTION_MESSAGE message;
        const eLeapRS result = LeapPollConnection(connection, timeout, &message);
        if (result != eLeapRS_Success) {
            std::cout << "Something went wrong!" << '\n';
            return 1;
        }
        if (message.device_id == 0 || message.tracking_event->nHands == 0) {
            continue;
        }

        const LEAP_HAND* hands = message.tracking_event->pHands;
        const uint32_t nHands = message.tracking_event->nHands;
        for (uint32_t i = 0; i < nHands; i++)
        {
            const LEAP_HAND hand = hands[i];
            if (hand.type == eLeapHandType_Right)
            {
                const LEAP_VECTOR hand_normal = hand.palm.normal;
                const LEAP_VECTOR hand_direction = hand.palm.direction;
                right_hand_roll = -hand_normal.x;
                right_hand_pitch = hand_normal.z;
                right_hand_yaw = hand_direction.x;
                
            } else
            {
                left_hand_pinch = hand.pinch_strength;
                left_hand_grab = hand.grab_strength;
                const LEAP_VECTOR left_hand_position = hand.palm.position;
                left_hand_x = left_hand_position.x;
                left_hand_y = left_hand_position.y;
                left_hand_z = left_hand_position.z;
            }
        }
        
    }
    return 0;
}

extern "C" {
    __declspec(dllexport) BOOL __cdecl initialize_connection ();
    __declspec(dllexport) BOOL __cdecl close_connection ();
    __declspec(dllexport) void __cdecl start_polling ();
    __declspec(dllexport) void __cdecl stop_polling ();
    __declspec(dllexport) float __cdecl get_right_hand_roll ();
    __declspec(dllexport) float __cdecl get_right_hand_pitch ();
    __declspec(dllexport) float __cdecl get_right_hand_yaw ();
    __declspec(dllexport) float __cdecl get_left_hand_pinch ();
    __declspec(dllexport) float __cdecl get_left_hand_grab ();
    __declspec(dllexport) float __cdecl get_left_hand_x ();
    __declspec(dllexport) float __cdecl get_left_hand_y ();
    __declspec(dllexport) float __cdecl get_left_hand_z ();
}

FLOAT get_right_hand_roll() { return right_hand_roll; }
FLOAT get_right_hand_pitch() { return right_hand_pitch; }
FLOAT get_right_hand_yaw() { return right_hand_yaw; }
FLOAT get_left_hand_pinch() { return left_hand_pinch; }
FLOAT get_left_hand_grab() { return left_hand_grab; }
FLOAT get_left_hand_x() { return left_hand_x; }
FLOAT get_left_hand_y() { return left_hand_y; }
FLOAT get_left_hand_z() { return left_hand_z; }

BOOL initialize_connection()
{
    eLeapRS result = LeapCreateConnection(nullptr, &connection);
    if (result != eLeapRS_Success) {
        std::cout << "Something went wrong!" << '\n';
        return false;
    }

    result = LeapOpenConnection(connection);
    if (result != eLeapRS_Success) {
        std::cout << "Something went wrong!" << '\n';
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
