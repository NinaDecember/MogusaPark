using UnityEngine;

public class TapPoint : MonoBehaviour
{
    public bool isSelect = false;
    [SerializeField] private GameObject canvasObject;

    [SerializeField] private CameraController cameraController;

    public void OnTapped()
    {
        if (isSelect) return;
        isSelect = true;

        cameraController.IsLocked = true;

        canvasObject.SetActive(true);
    }

    public void ClosePanel()
    {
        isSelect = false;

        cameraController.IsLocked = false; // Å© ÉJÉÅÉâçƒäJ

        canvasObject.SetActive(false);
    }

}
