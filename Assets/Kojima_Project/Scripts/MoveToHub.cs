using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToHub : MonoBehaviour
{
    public string gameSceneName = "HubScene";

    public void Move()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}