using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Unknown_Project
{
    public class PauseButtonController : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField] private LevelData levelData;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private Button titleButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button settingButton;

        private GameSceneState beforePauseGameState;


        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            pauseScreen.SetActive(false);

            beforePauseGameState = GameSceneState.Playing;
        }
        public void OnClickPauseButton()
        {
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
            SceneManager.LoadScene(levelData.hubSceneName);
        }

        public void OnClickRetryButton()
        {
            audioManager.PlaySE("Click");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnClickContinueButton()
        {
            audioManager.PlaySE("Click");
            audioManager.ResumeVoice();
            gameManager.SelectButtonInteractable();
            gameManager.SetGameState(beforePauseGameState);
            pauseScreen.SetActive(false);            
        }

        public void OnClickSettingButton()
        {
            
        }
    }
}
