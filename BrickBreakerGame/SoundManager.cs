using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace BrickBreakerGame
{
    public class SoundManager
    {
        private Dictionary<string, SoundEffect> soundEffects;
        private Song backgroundMusic;

        public SoundManager()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
        }

        public void LoadContent(ContentManager content)
        {
            // Indlæs lydeffekter
            soundEffects["wall_hit"] = content.Load<SoundEffect>("WallHit");
            soundEffects["ball_lost"] = content.Load<SoundEffect>("BallLost");
            soundEffects["brick_break"] = content.Load<SoundEffect>("BrickBreak");
            soundEffects["brick_hit"] = content.Load<SoundEffect>("BrickHit");
            soundEffects["game_over"] = content.Load<SoundEffect>("GameOver");
            soundEffects["level_win"] = content.Load<SoundEffect>("LevelWin");
            soundEffects["paddle_hit"] = content.Load<SoundEffect>("PaddleHit2");
            soundEffects["pick_up"] = content.Load<SoundEffect>("PickUp2");

            // Indlæs evt. baggrundsmusik
            backgroundMusic = content.Load<Song>("background_music"); // PH
        }

        public void PlaySound(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
            {
                soundEffects[soundName].Play();
            }
        }

        public void PlayBackgroundMusic()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        public void StopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
