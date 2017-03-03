using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class SoundManager
    {
        private Song song;
        private SoundEffect effect;
        private SoundEffectInstance effectInstance;

        private static Random rand = new Random();

        public Song Song
        {
            get { return song; }
            set { song = value; }
        }

        public SoundEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        //In order to have any control over a SoundEffect object while it's playing, we must convert it to a SoundEffectInstance
        //This makes our properties seem a little strange, but I promise it's better this way

        public float Volume
        {
            get { return effectInstance.Volume; }
            set { effectInstance.Volume = value; }
        }

        public float Pitch
        {
            get { return effectInstance.Pitch; }
            set { effectInstance.Pitch = value; }
        }

        public float Pan
        {
            get { return effectInstance.Pan; }
            set { effectInstance.Pan = value; }
        }

        /// <summary>
        /// Need help optimising this as a singleton
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="volume"></param>
        /// <param name="pan"></param>
        /// <param name="pitch"></param>
        /// <param name="song"></param>
        private SoundManager(SoundEffect effect = null, float volume = 0.0f, float pan = 0.0f, float pitch = 0.0f, Song song = null)
        {
            this.effect = effect;
            effectInstance = effect.CreateInstance();
            effectInstance.Volume = volume;
            effectInstance.Pan = pan;
            effectInstance.Pitch = pitch;
            this.song = song;
        }

        /// <summary>
        /// Calculates the ratio of the distance between an object and the player to the distance between the closest wall and the player
        /// </summary>
        /// <param name="player">Instance of the player</param>
        /// <param name="source">Entity object that acts as the source of the given sound effect</param>
        /// <param name="maxRange">Max distance the player can hear, should be about the radius of the room</param>
        public void CalculatePanAndVolume(Player player, Entity source, float maxRange)
        {
            Vector distance = new Vector(source.Position.X - player.Position.X, source.Position.Y - player.Position.Y);
            effectInstance.Pan = (float)(Math.Cos(distance.GetAngle()) / 2);
            effectInstance.Volume = (maxRange - distance.Magnitude()) / maxRange;
        }

        /// <summary>
        /// Generates a random change in pitch for the current sound effect between a half octave down and a half octave up
        /// </summary>
        public void RandomizePitch()
        {
            float temp = (float)rand.NextDouble();
            if (temp < 0.5f)
                effectInstance.Pitch = temp;
            else
                effectInstance.Pitch = temp - 1f;
        }

        /// <summary>
        /// Uses a MediaPlayer to start the current song
        /// </summary>
        /// <param name="songVolume">Desired volume of the song between 0.0f and 1.0f (serparate from the volume of the current sound effect)</param>
        public void PlaySong(float songVolume)
        {
            if (song != null)
            {
                MediaPlayer.Play(song);
                MediaPlayer.Volume = songVolume;
            }
            else
            {
                NullReferenceException nre = new NullReferenceException();
                throw (nre);
            }
        }

        /// <summary>
        /// Alters the volume of the current song (use for menus or fancy atmospheric effects)
        /// </summary>
        /// <param name="volumeChange">The ammount to add to the current volume, positive values increase volume, negative values decrease volume. Must keep volume within 0.0f and 1.0f</param>
        /// <returns>The current float value of the volume</returns>
        public float ChangeSongVolume(float volumeChange)
        {
            if (song != null)
            {
                if (MediaPlayer.Volume + volumeChange <= 1.0f && MediaPlayer.Volume + volumeChange >= 0.0f)
                {
                    MediaPlayer.Volume += volumeChange;
                    return MediaPlayer.Volume;
                }
                else
                {
                    Exception e = new Exception("Resulting volume out of bounds, must be between 0.0f and 1.0f. Current volume is " + MediaPlayer.Volume);
                    throw (e);
                }
            }
            else
            {
                NullReferenceException nre = new NullReferenceException();
                throw (nre);
            }
        }

        /// <summary>
        /// Stops the current effect (up for change) then calculates the proper values for the given effect before playing the effect
        /// </summary>
        /// <param name="effect">SoundEffect object to be played</param>
        /// <param name="player">Instance of the player</param>
        /// <param name="source">Entity that the effect originates from</param>
        /// <param name="maxRange">Maximum range that the player can hear</param>
        public void SetAndPlayEffect(SoundEffect effect, Player player, Entity source, float maxRange)
        {
            if (effectInstance.State == SoundState.Playing)
                effectInstance.Stop();
            this.effect = effect;
            effectInstance = effect.CreateInstance();
            CalculatePanAndVolume(player, source, maxRange);
            RandomizePitch();
            effectInstance.Play();
        }
    }
}
