using System;
using System.IO;
namespace BrickBreakerGame
{
    /// <summary>
    /// Håndtere logik relateret til spillets score og highscore.
    /// </summary>
    public class ScoreManager
    {
        // Fields
        private readonly string highScoreFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BrickBreakerGame",
            "highscore.txt");

        // Public property til at hente Score og HighScore
        public int Score { get; private set; }
        public int HighScore { get; private set; }

        /// <summary>
        /// Initialiserer ScoreManager og indlæser highscoren fra filen.
        /// </summary>
        public ScoreManager()
        {
            Score = 0;
            HighScore = LoadHighScore();
        }

        /// <summary>
        /// Tilføjer points til den nuværende score.
        /// Hvis scoren overstiger highscoren, så opdateres highsocren og den bliver gemt
        /// </summary>
        /// <param name="points"></param> Antal points der tilføjes til scoren.
        public void AddPoints(int points)
        {
            Score += points;
            if (Score > HighScore)
            {
                HighScore = Score;
                SaveHighScore();
            }
        }

        /// <summary>
        /// Nulstiller scoren til 0.
        /// </summary>
        public void ResetScore()
        {
            Score = 0;
        }

        /// <summary>
        /// Indlæser highscoren fra filen.
        /// </summary>
        /// <returns>Den indlæste highscore eller hvis filen ikke findes eller der opstår fejl, så returnerer den 0</returns>
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

        /// <summary>
        /// Gemmer den aktuelle highscore til filen.
        /// Hvis mappen ikke findes, så bliver den oprettet.
        /// </summary>
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
