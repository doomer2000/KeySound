using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeySound.Models {
    public class KeyboardHooker {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYUP = 0x0101;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void KeyboardHookCallback(Keys key);
        public static event KeyboardHookCallback KeyDown;
        public static event KeyboardHookCallback KeyUp;

        public KeyboardHooker() {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc) {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {

            if (nCode >= 0) {
                int vkCode = Marshal.ReadInt32(lParam);
                if (wParam == (IntPtr)WM_KEYDOWN) {
                    KeyDown?.Invoke((Keys)Marshal.ReadInt32(lParam));
                }
                if (wParam == (IntPtr)WM_KEYUP) {
                    KeyUp?.Invoke((Keys)Marshal.ReadInt32(lParam));
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
