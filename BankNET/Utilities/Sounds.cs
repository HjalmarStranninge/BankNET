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
        //To be able to use this sound you need to download the NAudio NuGet Package first.

        internal static void PlaySuccessSound()
        {
            // Replace "path_to_success_sound.wav" with the actual path to your success sound file.
            string successSoundFilePath = "C:\\Users\\sjood\\Downloads\\BankNET sounds\\Sounds_swish.Wav";
            PlaySound(successSoundFilePath);
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
