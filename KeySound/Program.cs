using System;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeySound.Models;

namespace KeySound {
    class Program {
        static void Main(string[] args) {
            KeyboardHooker.KeyUp += KeyboardHooker_KeyUp;
            KeyboardHooker.KeyDown += KeyboardHooker_KeyDown;
            KeyboardHooker keyboardHooker = new KeyboardHooker();
        }

        private static void KeyboardHooker_KeyDown(Keys key) {
            Console.WriteLine(key.ToString());
            SoundPlayer soundPlayer = new SoundPlayer();
            switch(key) {
                case Keys.Back:
                    soundPlayer.SoundLocation = "Sounds/backspace.wav";
                    break;
                case Keys.Space:
                    soundPlayer.SoundLocation = "Sounds/space.wav";
                    break;
                default:
                    soundPlayer.SoundLocation = "Sounds/a.wav";
                    break;

            }
            soundPlayer.Play();
        }

        private static void KeyboardHooker_KeyUp(Keys key) {
            //pass
        }
    }
}

