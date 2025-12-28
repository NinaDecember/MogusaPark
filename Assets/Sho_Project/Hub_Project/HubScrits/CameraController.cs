namespace HabScene
{
    using TMPro;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [Header("Camera Rotation")]
        [SerializeField] private float rotationSpeed = 0.2f;

        public bool IsLocked = false;   // Å© í«â¡

        private float lastTouchX;

       

        void Update()
        {
            if (IsLocked) return;   // Å© CanvasíÜÇÕÉJÉÅÉâëÄçÏã÷é~

            HandleTouchRotation();
        }

        void HandleTouchRotation()
        {
            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began)
                {
                    lastTouchX = t.position.x;
                }
                else if (t.phase == TouchPhase.Moved)
                {
                    float deltaX = t.position.x - lastTouchX;
                    transform.Rotate(Vector3.up, deltaX * rotationSpeed);
                    lastTouchX = t.position.x;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                float deltaX = Input.GetAxis("Mouse X");
                transform.Rotate(Vector3.up, deltaX * rotationSpeed);
            }
        }

        public void ChangePosition(Vector3 vec)
        {
            if (IsLocked) return;   // ÉçÉbÉNíÜÇÕà⁄ìÆã÷é~
            transform.position = vec;
        }
    }
}