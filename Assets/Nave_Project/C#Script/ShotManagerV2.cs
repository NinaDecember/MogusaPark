using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
public class ShotManagerV2 : MonoBehaviour
{
    public static ShotManagerV2 Instance;
    public ScreenShot screenShot;
    public FitCameraToObject fitCamera;
    public Button shot;
    public Button save;
    public Button cancel;
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
        save.enabled = false;
        cancel.enabled = false;
    }
    private void Update() {
        shot.onClick.AddListener(Shot);   
    }
    public void Shot()
    {
        screenShot.ClickShootButton();
        fitCamera.ShowUI();
        fitCamera.Fit();
        screenShot.ShowSSImage();
        StartCoroutine(IfSave());
        screenShot.DeleteSSImage();

    }
    private IEnumerator IfSave(){
        
        while(save.enabled ^ cancel.enabled){
        yield return null;
        }
        if(save.enabled){
            screenShot.SaveSSImage();
            yield break;
        }
        else if(cancel.enabled){
            yield break;
        }
    }
}