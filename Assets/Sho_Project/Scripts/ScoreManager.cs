namespace Sho_Project
{
    using TMPro;
    using UnityEngine;

    public class ScoreManager : MonoBehaviour
    {
        public int Score { get; private set; }
        private int highScore;

        void Awake()
        {
            LoadHighScore();
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public void AddScore(int value)
        {
            Score += value;
        }

        public void SaveHighScore()
        {
            if (Score > highScore)
            {
                highScore = Score;
                PlayerPrefs.SetInt("HighScore", highScore);
            }
        }

        void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        public int GetHighScore()
        {
            return highScore;
        }
    }
}
