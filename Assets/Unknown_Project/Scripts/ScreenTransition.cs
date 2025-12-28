using UnityEngine;
namespace Unknown_Project
{
    public class ScreenTransition : MonoBehaviour
    {
        [SerializeField] private GameObject fadeCanvas;
        private GameController gameController;
        private Animator fadeAnimator;


        private void Start()
        {
            gameController = FindFirstObjectByType<GameController>();

            fadeCanvas.GetComponent<CanvasGroup>().alpha = 1;
            fadeAnimator = fadeCanvas.GetComponent<Animator>();
        }

        public void StartFadeIn()
        {
            fadeAnimator.SetTrigger("StartFadeIn");
        }

        public void StartFadeOut()
        {
            fadeAnimator.SetTrigger("StartFadeOut");
        }

        public void FinishFadeIn()
        {
            Debug.Log("Finish");
            gameController.FinishFade();
        }
        public void FinishFadeOut()
        {
            Debug.Log("Finish");
            gameController.FinishFade();
        }




    }
}
