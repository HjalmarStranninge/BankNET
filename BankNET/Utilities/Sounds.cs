using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace BankNET.Utilities
{
    internal class Sounds
    {
        internal static void PlaySuccessSound()
        {
            // Replace "path_to_success_sound.wav" with the actual path to your success sound file.
            string successSoundFilePath = "C:\\Users\\sjood\\Downloads\\BankNET sounds\\mixkit-achievement-bell-600.wav";
            PlaySound(successSoundFilePath);
        }
        
        internal static void PlayErrorSound()
        {
            // Replace "path_to_error_sound.wav" with the actual path to your error sound file.
            string errorSoundFilePath = "C:\\Users\\sjood\\Downloads\\BankNET sounds\\negative_beeps-6008.mp3";
            PlaySound(errorSoundFilePath);
        }

        internal static void PlaySound(string soundFilePath)
        {
            if (System.IO.File.Exists(soundFilePath))
            {
                using (var audioFile = new AudioFileReader(soundFilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    // Block the program execution until the sound finishes playing
                    while (outputDevice.PlaybackState == PlaybackState.Playing) { }
                }
            }
            // You may want to handle the case where the sound file is not found.
        }
    }
}
