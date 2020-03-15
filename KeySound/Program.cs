using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeySound.Models;
using System.Runtime.InteropServices;

namespace KeySound {
    class Program {

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static void Main(string[] args) {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            KeyboardHooker.KeyUp += KeyboardHooker_KeyUp;
            KeyboardHooker.KeyDown += KeyboardHooker_KeyDown;
            KeyboardHooker keyboardHooker = new KeyboardHooker();
        }

        private static void KeyboardHooker_KeyDown(Keys key) {
            SoundPlayer soundPlayer = new SoundPlayer();
            switch(key) {
                case Keys.Enter:
                    soundPlayer.SoundLocation = "Sounds/Enter-key.wav";
                    break;
                case Keys.Back:
                    soundPlayer.SoundLocation = "Sounds/Backspace-key.wav";
                    break;
                case Keys.Space:
                    soundPlayer.SoundLocation = "Sounds/Space-key.wav";
                    break;
                default:
                    var random = new Random();
                    soundPlayer.SoundLocation = $"Sounds/{random.Next(0, 6)}-key.wav";
                    break;

            }
            soundPlayer.Play();
        }

        private static void KeyboardHooker_KeyUp(Keys key) {
            //pass
        }
    }
}

