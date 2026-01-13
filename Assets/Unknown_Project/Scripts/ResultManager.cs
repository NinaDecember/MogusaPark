using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Unknown_Project
{



    public class ResultManager : MonoBehaviour
    {
        private GameManager gameManager;
        private PythonClient pythonClient;
        [SerializeField] private LevelData levelData;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private ScreenTransition screenTransition;

        private double time;

        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            pythonClient = FindFirstObjectByType<PythonClient>();

            time = ResultData.endTime;
            ResultData.Reset();
        }

        public void OnClickReturnHub()
        {
            audioManager.PlaySE("Click");
            StartCoroutine(OnClickReturnHubFade());
        }
        private IEnumerator OnClickReturnHubFade()
        {
            yield return StartCoroutine(WaitFadeFinish());
            SceneManager.LoadScene(levelData.hubSceneName);
        }

        public void OnClickRetry()
        {
            audioManager.PlaySE("Click");
            StartCoroutine(OnClickRetryFade());
        }
        private IEnumerator OnClickRetryFade()
        {
            yield return StartCoroutine(WaitFadeFinish());
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        private IEnumerator WaitFadeFinish()
        {
            gameManager.SetIsFinishFade(false);
            pythonClient.OnDestroy();
            screenTransition.StartFadeIn();
            while (!gameManager.GetIsFinishFade())
            {
                yield return null;
            }
        }



    }
}
