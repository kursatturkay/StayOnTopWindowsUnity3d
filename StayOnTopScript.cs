using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace fablesalive
{
    [DisallowMultipleComponent]
    public class StayOnTop : MonoBehaviour
    {
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint TOPMOST_FLAGS = SWP_NOSIZE | SWP_NOMOVE;

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static bool instanceExists = false;

        void Awake()
        {
            if (instanceExists)
            {
                Destroy(gameObject);
                return;
            }
            instanceExists = true;
            DontDestroyOnLoad(gameObject);

#if !UNITY_EDITOR
        IntPtr windowHandle = GetForegroundWindow();
        SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
#endif
        }

        void OnDestroy()
        {
            instanceExists = false;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(StayOnTop))]
    public class StayOnTopEditor : Editor
    {
        void OnEnable()
        {
            StayOnTop[] instances = FindObjectsByType<StayOnTop>(FindObjectsSortMode.None);

            if (instances.Length > 1)
            {
                EditorUtility.DisplayDialog("Warning",
                    "Only one StayOnTop component can exist in a scene. The newly added instance will be removed.", "OK");

                Debug.LogError("Only one StayOnTop component can exist in a scene. The newly added instance is being removed.");
            }
        }
    }
#endif
}