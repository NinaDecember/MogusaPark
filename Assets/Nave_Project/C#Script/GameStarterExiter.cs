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
    public bool SystemSwitch = true; // true:アプリ終了、false:HubSceneへ移動
    void Start()
    {
        SetupEventSystem();
    }

    
    private void SetupEventSystem()
    {
        EventSystem es = FindObjectOfType<EventSystem>();
        if (es == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            es = esObj.AddComponent<EventSystem>();
            esObj.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(esObj);
        }
        else
        {
            DontDestroyOnLoad(es.gameObject);
        }

        // 最初の選択をリセット
        es.SetSelectedGameObject(null);
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
            if(SystemSwitch){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
                Application.OpenURL("https://unityroom.com/");
            #else
                Application.Quit();
            #endif
            }
            else{
            Destroy(EventSystem.current.gameObject);
            SceneManager.LoadScene("HubScene", LoadSceneMode.Single);
            }
        }
        }
    }
    }
}