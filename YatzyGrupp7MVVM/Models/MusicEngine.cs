using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YatzyGrupp7MVVM.Services;

namespace YatzyGrupp7MVVM.Models
{
    static class MusicEngine
    {
        public static string file { get; set; }
        static float volume = 0.12F;
        static float VolumeAtPause;
        static public WaveOut waveOut;
        static public bool isMute = false;

        static public void StartStop()
        {
            if (waveOut == null)
            {
                WaveFileReader reader = new WaveFileReader(file);
                LoopStream loop = new LoopStream(reader);
                waveOut = new WaveOut();
                waveOut.Init(loop);
                waveOut.Volume = volume;
                waveOut.Play();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }

        static public void Mute()
        {
            if (waveOut.Volume != 0F)
            {
                VolumeAtPause = volume;
                volume = 0F;
                waveOut.Volume = volume;
                isMute = true;
            }
            else if (waveOut.Volume == 0F)
            {
                volume = VolumeAtPause;
                waveOut.Volume = volume;
                isMute = false;
            }
        }

        static public void IncreaseVolume()
        {
            if (volume < 0.19F)
            {
                volume += 0.01F;
                waveOut.Volume = volume;
            }
          
        }

        static public void DecreaseVolume()
        {
            if (volume > 0.01F)
            {
                volume -= 0.01F;
                waveOut.Volume = volume;
            }
           
        }

        static IWavePlayer drawerPlayer;
        static AudioFileReader fileDrawerPlayer;
        static public void DrawerPlayerMethod()
        {
            string filename2 = "Media/Sounds/drawer.wav";
            drawerPlayer = new WaveOut();
            fileDrawerPlayer = new AudioFileReader(filename2);
            drawerPlayer.Init(fileDrawerPlayer);
            drawerPlayer.Play();
        }

        static public void ButtonSoundEffect()
        {
            IWavePlayer soundEffectButtonClick;
            AudioFileReader fileSoundEffectButtonClick;
            string filename3 = "Media/Sounds/buttonhit.wav";
            soundEffectButtonClick = new WaveOut();
            fileSoundEffectButtonClick = new AudioFileReader(filename3);
            soundEffectButtonClick.Init(fileSoundEffectButtonClick);
            soundEffectButtonClick.Play();
        }

        static public void ButtonSoundEffectGameStart()
        {
            IWavePlayer soundEffectButtonClick;
            AudioFileReader fileSoundEffectButtonClick;
            string filename3 = "Media/Sounds/StartGameSoundEffect.wav";
            soundEffectButtonClick = new WaveOut();
            fileSoundEffectButtonClick = new AudioFileReader(filename3);
            soundEffectButtonClick.Init(fileSoundEffectButtonClick);
            soundEffectButtonClick.Play();
        }
    }
}
