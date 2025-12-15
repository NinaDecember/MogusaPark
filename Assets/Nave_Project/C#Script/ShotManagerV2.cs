using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
namespace Nave_Project
{
public class ShotManagerV2 : MonoBehaviour
{
    public static ShotManagerV2 Instance;
    public ScreenShot screenShot;
    public FitCameraToObject fitCamera;
    public Button shot;
    public Button save;
    public Button cancel;
    public Button Retry;
    public Button GoToTitle;
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if(SceneManager.GetActiveScene().name != "BestShot_Game")
        {
         shot.enabled = false;
         shot.gameObject.SetActive(false);
         Destroy(fitCamera.target);   
        }
    }
    void Start()
    {
        screenShot = GetComponent<ScreenShot>();
        fitCamera = GetComponent<FitCameraToObject>();
        shot.enabled = true;
        save.enabled = false;
        cancel.enabled = false;
        Retry.enabled = false;
        GoToTitle.enabled = false;
        fitCamera.HideUI();
        save.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        Retry.gameObject.SetActive(false);
        GoToTitle.gameObject.SetActive(false);
        shot.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void Update() {
        shot.onClick.AddListener(Shot);   
    }
    public void Shot()
    {
        screenShot.ClickShootButton();
        fitCamera.ShowUI();
        shot.gameObject.SetActive(false);
        screenShot.ShowSSImage();
        fitCamera.Fit();
        StartCoroutine(IfTitle());
        EventSystem.current.SetSelectedGameObject(null);
    }
    private IEnumerator IfSave(){
        save.gameObject.SetActive(true);
        cancel.gameObject.SetActive(true);
        save.enabled = true;
        cancel.enabled = true;
        while (save.enabled && cancel.enabled){
        if(EventSystem.current.currentSelectedGameObject == save.gameObject){
            screenShot.SaveSSImage();
            save.enabled = false;
            cancel.enabled = false;
            screenShot.DeleteSSImage();
            break;
        }
        else if(EventSystem.current.currentSelectedGameObject == cancel.gameObject)
        {
            save.enabled = false;
            cancel.enabled = false;
            screenShot.DeleteSSImage();
            break;
        }
            yield return null;
        }
        save.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        save.enabled = false;
        cancel.enabled = false;
    }
        private IEnumerator IfTitle()
    {
        yield return StartCoroutine(IfSave());
        Retry.gameObject.SetActive(true);
        GoToTitle.gameObject.SetActive(true);
        Retry.enabled = true;
        GoToTitle.enabled = true;
        while (Retry.enabled && GoToTitle.enabled)
        {
            if (EventSystem.current.currentSelectedGameObject == Retry.gameObject)
            {
                Retry.enabled = false;
                GoToTitle.enabled = false;
                SceneManager.LoadScene("BestShot_Game");
                break;
            }
            else if (EventSystem.current.currentSelectedGameObject == GoToTitle.gameObject)
            {
                Retry.enabled = false;
                GoToTitle.enabled = false;
                SceneManager.LoadScene("BestShot_Title", LoadSceneMode.Single);
                break;
            }
            yield return null;
        }
        Retry.gameObject.SetActive(false);
        GoToTitle.gameObject.SetActive(false);
        Retry.enabled = false;
        GoToTitle.enabled = false;
    }
}
}