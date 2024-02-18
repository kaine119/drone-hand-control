#include <iostream>
#include <windows.h>
#include "LeapC.h"

static bool is_thread_running = false;

static LEAP_CONNECTION connection;
static HANDLE thread;
static DWORD thread_handle;

static int right_hand_count = 0; // nominally 1
static float right_hand_roll;
static float right_hand_pitch;
static float right_hand_yaw;
static float right_hand_x;
static float right_hand_y;
static float right_hand_z;
static float right_hand_grab;
static float right_hand_pinch;

static int left_hand_count;
static float left_hand_x;
static float left_hand_y;
static float left_hand_z;
static float left_hand_roll;
static float left_hand_pitch;
static float left_hand_yaw;
static float left_hand_grab;
static float left_hand_pinch;



static DWORD WINAPI run_thread(LPVOID params)
{
    while (is_thread_running) {
        constexpr unsigned int timeout = 300000;
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
        const uint32_t n_hands = message.tracking_event->nHands;
        right_hand_count = 0;
        left_hand_count = 0;
        
        for (uint32_t i = 0; i < n_hands; i++)
        {
            const LEAP_HAND hand = hands[i];
            if (hand.type == eLeapHandType_Right)
            {
                right_hand_count += 1;
                const LEAP_VECTOR normal = hand.palm.normal;
                const LEAP_VECTOR direction = hand.palm.direction;
                const LEAP_VECTOR position = hand.palm.position;
                right_hand_x = position.x;
                right_hand_y = position.y;
                right_hand_z = position.z;
                right_hand_roll = -normal.x;
                right_hand_pitch = normal.z;
                right_hand_yaw = direction.x;
                right_hand_grab = hand.grab_strength;
                right_hand_pinch = hand.pinch_strength;
            } else
            {
                left_hand_count += 1;
                const LEAP_VECTOR normal = hand.palm.normal;
                const LEAP_VECTOR direction = hand.palm.direction;
                const LEAP_VECTOR position = hand.palm.position;
                left_hand_x = position.x;
                left_hand_y = position.y;
                left_hand_z = position.z;
                left_hand_roll = -normal.x;
                left_hand_pitch = normal.z;
                left_hand_yaw = direction.x;
                left_hand_grab = hand.grab_strength;
                left_hand_pinch = hand.pinch_strength;
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

    
    __declspec(dllexport) int __cdecl get_right_hand_count ();
    __declspec(dllexport) float __cdecl get_right_hand_x ();
    __declspec(dllexport) float __cdecl get_right_hand_y ();
    __declspec(dllexport) float __cdecl get_right_hand_z ();
    __declspec(dllexport) float __cdecl get_right_hand_roll ();
    __declspec(dllexport) float __cdecl get_right_hand_pitch ();
    __declspec(dllexport) float __cdecl get_right_hand_yaw ();
    __declspec(dllexport) float __cdecl get_right_hand_pinch ();
    __declspec(dllexport) float __cdecl get_right_hand_grab ();
    
    __declspec(dllexport) int __cdecl get_left_hand_count ();
    __declspec(dllexport) float __cdecl get_left_hand_x ();
    __declspec(dllexport) float __cdecl get_left_hand_y ();
    __declspec(dllexport) float __cdecl get_left_hand_z ();
    __declspec(dllexport) float __cdecl get_left_hand_roll ();
    __declspec(dllexport) float __cdecl get_left_hand_pitch ();
    __declspec(dllexport) float __cdecl get_left_hand_yaw ();
    __declspec(dllexport) float __cdecl get_left_hand_pinch ();
    __declspec(dllexport) float __cdecl get_left_hand_grab ();
}

INT get_right_hand_count() { return right_hand_count; }
FLOAT get_right_hand_x() { return right_hand_x; }
FLOAT get_right_hand_y() { return right_hand_y; }
FLOAT get_right_hand_z() { return right_hand_z; }
FLOAT get_right_hand_roll() { return right_hand_roll; }
FLOAT get_right_hand_pitch() { return right_hand_pitch; }
FLOAT get_right_hand_yaw() { return right_hand_yaw; }
FLOAT get_right_hand_pinch() { return right_hand_pinch; }
FLOAT get_right_hand_grab() { return right_hand_grab; }

INT get_left_hand_count() { return left_hand_count; }
FLOAT get_left_hand_x() { return left_hand_x; }
FLOAT get_left_hand_y() { return left_hand_y; }
FLOAT get_left_hand_z() { return left_hand_z; }
FLOAT get_left_hand_roll() { return left_hand_roll; }
FLOAT get_left_hand_pitch() { return left_hand_pitch; }
FLOAT get_left_hand_yaw() { return left_hand_yaw; }
FLOAT get_left_hand_pinch() { return left_hand_pinch; }
FLOAT get_left_hand_grab() { return left_hand_grab; }

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
