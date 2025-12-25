using Sho_Project;
using UnityEngine;
namespace Unknown_Project
{
    public class PauseButtonController : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField] private AudioManager audioManager;


        private void Start()
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        public void OnClick()
        {
            audioManager.PlaySE("Click");
            gameManager.SetGameState(GameSceneState.Pause);
        }
    }
}
