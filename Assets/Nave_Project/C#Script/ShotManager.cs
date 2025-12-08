using UnityEngine;
using System;
using UnityEngine.UI;
public class ShotManager : MonoBehaviour
{
    public ScreenShot screenShot;
    public FitCameraToObject fitCamera;
    public RawImage raw;
    
    void Start()
    {
        Button shotbutton = GetComponent<Button>();
        Button savebutton = GetComponent<Button>();
        Button delebutton = GetComponent<Button>();
        raw = gameObject.GetComponent<RawImage>();
    }
    public void Shot()
    {
        
        screenShot.ClickShootButton();
        fitCamera.ShowUI();
        fitCamera.Fit();
    }
}