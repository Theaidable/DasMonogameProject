using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace BrickBreakerGame
{
    /// <summary>
    /// Hpndterer lydeffekter og baggrundsmusik i spillet.
    /// </summary>
    public class SoundManager
    {
        // Fields
        private Dictionary<string, SoundEffect> soundEffects;
        //private Song backgroundMusic;

        /// <summary>
        /// Initialiserer en ny instans af SoundManager.
        /// </summary>
        public SoundManager()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
        }

        /// <summary>
        /// Indlæser lydeffekt og musik
        /// </summary>
        /// <param name="content"></param> ContentManager bruges til at indlæse lyden og musikken
        public void LoadContent(ContentManager content)
        {
            // Indlæs lydeffekter
            soundEffects["wall_hit"] = content.Load<SoundEffect>("Sounds/WallHit");
            soundEffects["ball_lost"] = content.Load<SoundEffect>("Sounds/BallLost");
            soundEffects["brick_break"] = content.Load<SoundEffect>("Sounds/BrickBreak");
            soundEffects["brick_hit"] = content.Load<SoundEffect>("Sounds/BrickHit");
            soundEffects["game_over"] = content.Load<SoundEffect>("Sounds/GameOver");
            soundEffects["level_win"] = content.Load<SoundEffect>("Sounds/LevelWin");
            soundEffects["paddle_hit"] = content.Load<SoundEffect>("Sounds/PaddleHit2");
            soundEffects["pick_up"] = content.Load<SoundEffect>("Sounds/PickUp2");

            // Indlæs evt. baggrundsmusik
            // backgroundMusic = content.Load<Song>("background_music"); // PH
        }

        /// <summary>
        /// Afspiller en specifik lydeffekt baseret på navnet.
        /// </summary>
        /// <param name="soundName"></param> Navnet på lydeffekten, der skal afspilles.
        public void PlaySound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                soundEffects[soundName].Play();
            }
        }

        //public void PlayBackgroundMusic()
        //{
        //    MediaPlayer.IsRepeating = true;
        //    MediaPlayer.Play(backgroundMusic);
        //}

        //public void StopBackgroundMusic()
        //{
        //    MediaPlayer.Stop();
        //}
    }
}
