using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Unknown_Project
{
    public class StartButtonController : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;

        public void OnClick()
        {
            audioManager.PlaySE("Click");
            Time.timeScale = 1f;
            StartCoroutine(TimeLug());
        }

        private IEnumerator TimeLug()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("PuzzleGameScene");
        }
    }
}
