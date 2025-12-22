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
    public Canvas Canvas;
    private IEnumerator IfSave()
    {
        Canvas.gameObject.SetActive(true);
        save.gameObject.SetActive(true);
        cancel.gameObject.SetActive(true);
        save.enabled = true;
        cancel.enabled = true;
        while (save.enabled && cancel.enabled)
        {
            if (EventSystem.current.currentSelectedGameObject == save.gameObject)
            {
                screenShot.SaveSSImage();
                save.enabled = false;
                cancel.enabled = false;
                break;
            }
            else if (EventSystem.current.currentSelectedGameObject == cancel.gameObject)
            {
                save.enabled = false;
                cancel.enabled = false;
                break;
            }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    Canvas.gameObject.SetActive(true);
                }
            yield return null;
        }
        save.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        //save.enabled = false;
        //cancel.enabled = false;
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
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    Canvas.gameObject.SetActive(true);
                }
                yield return null;
        }
        Retry.gameObject.SetActive(false);
        GoToTitle.gameObject.SetActive(false);
            //Retry.enabled = false;
            //GoToTitle.enabled = false;
            fitCamera.HideUI();

        }
        private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
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
        if (SceneManager.GetActiveScene().name != "BestShot_Game")
        {
            shot.enabled = false;
            shot.gameObject.SetActive(false);
            Destroy(fitCamera.target);
        }
            fitCamera.HideUI();
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
        screenShot.ResetCreatedFlag();
    }
    
    async public void Shot()
    {
        screenShot.ClickShootButton();
        shot.gameObject.SetActive(false);
        await System.Threading.Tasks.Task.Delay(600); //少し待つ
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(IfTitle());
        prevwiew();
        
    }
    async void prevwiew()
    {
        fitCamera.ShowUI();
        //fitCamera.Fit();
        screenShot.ShowSSImage();

    }
    }}