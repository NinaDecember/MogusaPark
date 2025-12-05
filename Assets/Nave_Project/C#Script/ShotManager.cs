using UnityEngine;
using System;
public class ShotManager : MonoBehaviour
{
    public ScreenShot screenShot;
    public FitCameraToObject fitCamera;
    void Start()
    {
        screenShot = GetComponent<ScreenShot>();
        fitCamera = GetComponent<FitCameraToObject>();
        
    }
    public void Shot()
    {
        
        screenShot.ClickShootButton();
        fitCamera.ShowUI();
        fitCamera.Fit();
    }
}