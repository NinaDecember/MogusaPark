using UnityEngine;

public class TapDetector : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // タップ（スマホ）
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckTap(Input.GetTouch(0).position);
        }

        // クリック（PC）
        if (Input.GetMouseButtonDown(0))
        {
            CheckTap(Input.mousePosition);
        }
    }

    void CheckTap(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // タップされたオブジェクトに TapPoint がついていたら呼ぶ
            var tapPoint = hit.collider.GetComponent<TapPoint>();
            if (tapPoint != null)
            {
                tapPoint.OnTapped();
            }
        }
    }
}
