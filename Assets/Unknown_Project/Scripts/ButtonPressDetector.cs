//make by chatGPT

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ButtonController ctrlB;
    public bool isPushed;

    private Vector2 initPos;

    private void Start()
    {
        ctrlB = FindFirstObjectByType<ButtonController>();
        isPushed = false;
        initPos = GetComponent<RectTransform>().anchoredPosition;
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


    public void SetInitPos()
    {
        initPos = GetComponent<RectTransform>().anchoredPosition;
    }

    public Vector2 GetInitPos()
    {
        return initPos;
    }

    public Vector2 GetReturnInitPos()
    {
        return initPos;
    }
}
