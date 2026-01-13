using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Unknown_Project
{
    public class PauseButtonController : MonoBehaviour
    {
        private GameManager gameManager;
        private PythonClient pythonClient;
        [SerializeField] private LevelData levelData;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private ScreenTransition screenTransition;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject settingScreen;
        [SerializeField] private Button titleButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button settingButton;

        private GameSceneState beforePauseGameState;
        private bool isPaused;


        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            pythonClient = FindFirstObjectByType<PythonClient>();
            pauseScreen.SetActive(false);

            beforePauseGameState = GameSceneState.Playing;
            isPaused = false;
        }
        public void OnClickPauseButton()
        {
            if(isPaused)return;
            isPaused = true;
            audioManager.PlaySE("Click");
            audioManager.StopVoice();
            beforePauseGameState = gameManager.GetGameState();
            gameManager.SelectButtonInteractDisable();
            gameManager.SetGameState(GameSceneState.Pause);
            pauseScreen.SetActive(true);
        }

        public void OnClickTitleButton()
        {
            audioManager.PlaySE("Click");
            StartCoroutine(OnClickTitleButtonFade());

        }
        private IEnumerator OnClickTitleButtonFade()
        {
            yield return StartCoroutine(WaitFadeFinish());
            Time.timeScale = 1f;
            SceneManager.LoadScene(levelData.hubSceneName);
        }

        public void OnClickRetryButton()
        {
            audioManager.PlaySE("Click");
            StartCoroutine(OnClickRetryButtonFade());
        }
        private IEnumerator OnClickRetryButtonFade()
        {
            yield return StartCoroutine(WaitFadeFinish());
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnClickContinueButton()
        {
            isPaused = false;
            audioManager.PlaySE("Click");
            audioManager.ResumeVoice();
            gameManager.SelectButtonInteractable();
            gameManager.SetGameState(beforePauseGameState);
            pauseScreen.SetActive(false);            
        }

        public void OnClickSettingButton()
        {
            audioManager.PlaySE("Click");
            settingScreen.SetActive(true);
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
