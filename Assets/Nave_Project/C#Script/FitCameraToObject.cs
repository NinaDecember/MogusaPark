using UnityEngine;
using UnityEngine.UI;
public class FitCameraToObject : MonoBehaviour
{
    public Camera cam;
    public RawImage target;
    void Fit()
    {
        if (cam == null) cam = Camera.main;

        // カメラ表示範囲
        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        RectTransform rt = target.rectTransform;
        rt.sizeDelta = new Vector2(width, height);
        rt.anchoredPosition = Vector2.zero; // 中央基準に
    }
}
