using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Unknown_Project
{

    public class ReturnHubButtonController : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        public void OnClick()
        {
            audioManager.PlaySE("Click");
            StartCoroutine(TimeLug());
        }

        private IEnumerator TimeLug()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("TopScene");
        }
    }
}