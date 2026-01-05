namespace HabScene
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class CameraController : MonoBehaviour
    {
        [Header("Camera Rotation")]
        [SerializeField] private float pcRotationSpeed = 2f;      // マウス用
        [SerializeField] private float touchRotationSpeed = 100f; // タッチ用（画面幅で割って調整）

        public bool IsLocked = false;

        private float lastTouchX;

        void Update()
        {
            if (IsLocked) return;

#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseRotation();
#elif UNITY_IOS || UNITY_ANDROID
            HandleTouchRotation();
#endif
        }

        // --- PC用マウス操作 ---
        void HandleMouseRotation()
        {
            if (Input.GetMouseButton(0))
            {
                float deltaX = Input.GetAxis("Mouse X");
                transform.Rotate(Vector3.up, deltaX * pcRotationSpeed);
            }
        }

        // --- スマホ用タッチ操作 ---
        void HandleTouchRotation()
        {
            if (Input.touchCount != 1) return;

            Touch t = Input.GetTouch(0);

            // UI上なら回転しない
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(t.fingerId))
                return;

            if (t.phase == TouchPhase.Began)
            {
                lastTouchX = t.position.x;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                // deltaXを画面幅で正規化して回転
                float deltaX = (t.position.x - lastTouchX) / Screen.width;
                transform.Rotate(Vector3.up, deltaX * touchRotationSpeed);
                lastTouchX = t.position.x;
            }
        }

        // --- カメラ位置変更 ---
        public void ChangePosition(Vector3 vec)
        {
            if (IsLocked) return;
            transform.position = vec;
        }
    }
}