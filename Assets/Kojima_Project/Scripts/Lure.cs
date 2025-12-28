using UnityEngine;

namespace Koji
{
    public class Lure : MonoBehaviour
    {
        public bool isGrounded = false;

        void OnCollisionEnter(Collision col)
        {
            if (col.collider.CompareTag("Water"))
            {
                isGrounded = true;
                // ���ɕ�������
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;       // ���܂Ȃ�
                rb.isKinematic = true;       // �Î~
            }
        }
    }

}

