using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public string gameSceneName = "MainScene";

    public void Retry()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}