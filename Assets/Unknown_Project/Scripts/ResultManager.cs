using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Unknown_Project
{



    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private AudioManager audioManager;

        private double time;

        private void Start()
        {
            time = ResultData.endTime;
            ResultData.Reset();
        }

        public void OnClickReturnHub()
        {
            audioManager.PlaySE("Click");
            SceneManager.LoadScene(levelData.hubSceneName);
        }

        public void OnClickRetry()
        {
            audioManager.PlaySE("Click");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
