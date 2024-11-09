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
            // Find/Producer de lydeffekter der skal bruges
            
            // Indlæs lydeffekter
            soundEffects["brick_hit"] = content.Load<SoundEffect>("brick_hit");     // PH
            soundEffects["paddle_hit"] = content.Load<SoundEffect>("paddle_hit");   // PH
            soundEffects["ball_loss"] = content.Load<SoundEffect>("ball_loss");     // PH

            // Indlæs baggrundsmusik
            backgroundMusic = content.Load<Song>("background_music");               // PH
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
            MediaPlayer.IsRepeating = true; // Gentag musikken
            MediaPlayer.Play(backgroundMusic);
        }

        public void StopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
