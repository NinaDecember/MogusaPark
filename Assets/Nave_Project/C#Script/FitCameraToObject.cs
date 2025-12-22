using UnityEngine;
using UnityEngine.UI;
namespace Nave_Project
{
public class FitCameraToObject : MonoBehaviour
{
    public Camera cam;
    public RawImage target;
    public void Fit()
    {
        if (cam == null) cam = Camera.main;
        // カメラ表示範囲
        float height = Camera.main.orthographicSize * 2f;
        float width = height * cam.aspect;
        //target.rect = new Rect(0, 0, width, height);
        }
    public void ShowUI()
    {
        target.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        target.gameObject.SetActive(false);
    }
}}