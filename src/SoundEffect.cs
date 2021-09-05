using System;
using System.IO;

namespace Invaders
{
    public class SoundEffect : Audio
    {
        private bool Random = false;

        private Int64 Volume = 0;

        private int SoundIndex = 0;
        private int SoundCount = 0;

        private Stream[] Sounds = null;

        private Random Aleatory = null;

        public SoundEffect(Stream[] Sounds, Int64 Volume, bool Random)
        {
            if (null == Sounds) return;
            
            this.Sounds = Sounds;

            this.SoundCount = Sounds.Length;
            this.SoundIndex = SoundCount;

            this.Volume = Volume;

            this.Random = Random;

            Aleatory = new Random((int)DateTime.Now.Ticks);
        }

        public void Play()
        {
            if (null == Sounds) return;

            Play(Wave, Volume);
        }

        public void Play(Int64 Volume)
        {
            if (null == Sounds) return;

            Play(Wave, Volume);
        }

        private Stream Wave
        {
            get
            {
                if (1 == SoundCount)
                {
                    return (Sounds[0]);
                }

                if (Random)
                {
                    return (Sounds[Aleatory.Next(SoundCount)]);
                }

//              then cycling
                {
                    Stream Wave = Sounds[SoundIndex %= SoundCount];

                    ++SoundIndex;

                    return (Wave);
                }
            }
        }
    }
}
