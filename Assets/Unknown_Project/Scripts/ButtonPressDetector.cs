//make by chatGPT

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ButtonController ctrlB;
    public bool isPushed;

    private void Start()
    {
        ctrlB = FindFirstObjectByType<ButtonController>();
        isPushed = false;
    } 
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("押された！");
        isPushed = true;
        ctrlB.Pushed(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("離された！");
        isPushed = false;
    }
}
