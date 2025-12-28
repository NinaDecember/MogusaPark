namespace Sho_Project
{

    using System.Collections;
    using UnityEngine;

    public class IdolController : MonoBehaviour
    {
        [SerializeField] private ItemManager itemManager;
        [SerializeField] private Vector3[] movePoints = new Vector3[3];
        private int orderA;
        private int orderB;
        [SerializeField] private Animator idolAnim;
        /// <summary>
        /// 注文を受け取って即1つを選ぶ
        /// </summary>
        public void SetOrder(int a, int b)
        {
            orderA = a;
            orderB = b;

            // 2つの中からランダムで1つ選ぶ
            int choice = (Random.Range(0, 2) == 0) ? orderA : orderB;
            itemManager.IdolPick(choice);
            Debug.Log("アイドルが選んだ番号は：" + choice);
            //Debug.Log($"【Idol】注文 {orderA} / {orderB} から {choice} を選びました");

            StartCoroutine(IdolMove(movePoints[choice]));
        }

        private IEnumerator IdolMove(Vector3 target)
        {

            Vector3 start = transform.position;
            float elapsed = 0f;
            float moveTime = 0.15f;

            while (elapsed < moveTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / moveTime;
                t = t * t * (3 - 2 * t); // スムーズステップ
                transform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }

            transform.position = target;
            idolAnim.SetBool("IsLiftUp", true);
        }
    }

}