using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Nave_Project
{   
public class GameStarterExiter : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        SceneManager.UnloadSceneAsync("BestShot_Game");
        if (SceneManager.GetActiveScene().name == this.gameObject.scene.name)
        {
            StartButton.gameObject.SetActive(true);
            ExitButton.gameObject.SetActive(true);
            StartButton.enabled = true;
            ExitButton.enabled = true;
        }
            EventSystem.current.SetSelectedGameObject(null);
            if(EventSystem.current.gameObject.scene.name != "DontDestroyOnLoad")
                {DontDestroyOnLoad(EventSystem.current.gameObject);}
            else{
                Destroy(EventSystem.current.gameObject);
                DontDestroyOnLoad(EventSystem.current.gameObject);
            }

        }

        public void GameStart()
    {
        Debug.Log("GameStart");
        SceneManager.LoadScene("BestShot_Game", LoadSceneMode.Single);
        StartButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        StartButton.enabled = false;
        ExitButton.enabled = false;
   }
    public void GameExit()
    {
        if(SceneManager.GetActiveScene().name == "BestShot_Title")
        {
            Debug.Log("GameExit");
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
                Application.OpenURL("https://unityroom.com/");
            #else
                Application.Quit();
            #endif
        }
        }
    }
    }
}