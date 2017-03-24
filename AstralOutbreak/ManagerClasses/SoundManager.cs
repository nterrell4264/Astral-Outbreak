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
        //Creates an instance if SoundManager
        private static SoundManager instance;

        //Song objects will play consistently behind the sound effects. If soundmanager is loaded when the current room is it's possible the song will start over every time a room is entered
        private Song song;
        //Creating a dictionary to store SoundEffectInstances with string names as keys
        private Dictionary<String, SoundEffectInstance> effects;

        private static Random rand = new Random();

        /// <summary>
        /// Returns instance, or creates a new one if it's null
        /// </summary>
        public static SoundManager Instance
        {
            get {
                if(instance == null)
                {
                    instance = new SoundManager();
                }
                return instance; }
        }

        //We can use these properties for testing, not sure if they're neccessary yet

        public Song Song
        {
            get { return song; }
            set { song = value; }
        }

        /// <summary>
        /// Essentially a blank contructor
        /// </summary>
        /// <param name="song">Song to set as the current song</param>
        private SoundManager(Song song = null)
        {
            this.song = song;
        }

        /// <summary>
        /// Calculates the ratio of the distance between an object and the player to the distance between the closest wall and the player
        /// </summary>
        /// <param name="name">Name of the sound effect to change the pan and volume of</param>
        /// <param name="player">Instance of the player</param>
        /// <param name="source">Entity object that acts as the source of the given sound effect</param>
        /// <param name="maxRange">Max distance the player can hear, should be about the radius of the room</param>
        public void CalculatePanAndVolume(string name, Player player, Entity source, float maxRange)
        {
            Vector distance = new Vector(source.Position.X - player.Position.X, source.Position.Y - player.Position.Y);
            effects[name].Pan = (float)(Math.Cos(distance.GetAngle()) / 2);
            effects[name].Volume = (maxRange - distance.Magnitude()) / maxRange;
        }

        /// <summary>
        /// Generates a random change in pitch for the current sound effect between a half octave down and a half octave up
        /// </summary>
        /// <param name="name">Name of the sound effect to change the pitch of</param>
        public void RandomizePitch(string name)
        {
            float temp = (float)rand.NextDouble();
            //Makes sure the random pitch stays within -.5 and .5
            if (temp < 0.5f)
                effects[name].Pitch = temp;
            else
                effects[name].Pitch = temp - 1f;
        }

        /// <summary>
        /// Uses a MediaPlayer to start the current song, use whenever the song is changed
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
                //Ensures the resulting volume will be between 0 and 1
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
        /// Adds the desired sound effect to the dictionary of effects with the name as the key
        /// </summary>
        /// <param name="name">Name to denote the sound effect as the key</param>
        /// <param name="effect">Sound effect to be added and converted to a sound effect instance</param>
        public void AddEffect(string name, SoundEffect effect)
        {
            //Checks if name is unused and effect exists
            if(!effects.ContainsKey(name) && effect != null)
            {
                //Converts the sound effect to a sound effect instance before adding to give us more control over the effect
                effects.Add(name, effect.CreateInstance());
            }
        }

        /// <summary>
        /// Plays a sound effect with its current pan, pitch, and volume
        /// </summary>
        /// <param name="name">Name of the sound effect in the dictionary</param>
        public void PlayEffect(string name)
        {
            //Checks if the key exists
            if (effects.ContainsKey(name))
            {
                effects[name].Play(); 
            }
        }
    }
}
