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
    void Update()
    {
        
        StartCoroutine(screenShot.CreateScreenShot());
        
    }
}