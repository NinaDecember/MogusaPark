using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
namespace Nave_Project.CSharpScript
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
    }
    void Start()
    {
        screenShot = GetComponent<ScreenShot>();
        fitCamera = GetComponent<FitCameraToObject>();
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
        save.gameObject.SetActive(true);
        cancel.gameObject.SetActive(true);
        shot.gameObject.SetActive(false);
        save.enabled = true;
        cancel.enabled = true;
        fitCamera.Fit();
        screenShot.ShowSSImage();
        StartCoroutine(IfTitle());
        EventSystem.current.SetSelectedGameObject(null);
    }
    private IEnumerator IfSave(){
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
    }
    private IEnumerator IfTitle()
    {
        yield return StartCoroutine(IfSave());
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
                SceneManager.LoadScene("BestShot_Title");
                break;
            }
            yield return null;

        }
    }
}
}