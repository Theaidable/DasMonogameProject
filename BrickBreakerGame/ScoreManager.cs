using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreakerGame
{
    public class ScoreManager
    {
        public int Score { get; private set; }
        public int HighScore { get; private set; }

        public ScoreManager()
        {
            Score = 0;
            HighScore = 0;
        }

    }
