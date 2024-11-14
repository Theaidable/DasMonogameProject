using System;
using System.IO;
namespace BrickBreakerGame
{
    public class ScoreManager
    {
        public int Score { get; private set; }
        public int HighScore { get; private set; }

        private readonly string highScoreFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BrickBreakerGame",
            "highscore.txt"
);
        public ScoreManager()
        {
            Score = 0;
            HighScore = LoadHighScore();
        }

        public void AddPoints(int points)
        {
            Score += points;
            if (Score > HighScore)
            {
                HighScore = Score;
                SaveHighScore();
            }
        }

        public void ResetScore()
        {
            Score = 0;
        }

        private int LoadHighScore()
        {
            try
            {
                if (File.Exists(highScoreFilePath))
                {
                    string highScoreText = File.ReadAllText(highScoreFilePath);
                    if (int.TryParse(highScoreText, out int loadedHighScore))
                    {
                        return loadedHighScore;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading high score: {ex.Message}");
            }
            return 0; // Returner 0, hvis ingen highscore er gemt
        }

        private void SaveHighScore()
        {
            try
            {
                string directory = Path.GetDirectoryName(highScoreFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(highScoreFilePath, HighScore.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving high score: {ex.Message}");
            }
        }

    }
}
