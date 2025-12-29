namespace Sho_Project
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class Customer : MonoBehaviour
    {
        private ItemManager itemManager;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image[] images;
        [Header("Item Pool")]
        public int[] itemPool = new int[] { 0, 1, 2 };

        [SerializeField] private float moveDuration = 0.5f;
        public void Init(ItemManager itemManager)
        {
            this.itemManager = itemManager;
        }
        // CustomerSpawner から呼ばれた時に移動
        public void MoveTo(Vector3 targetPos)
        {
            StartCoroutine(MoveCoroutine(targetPos));

        }

        private IEnumerator MoveCoroutine(Vector3 targetPos)
        {
            Vector3 startPos = transform.position;
            float time = 0f;

            while (time < moveDuration)
            {
                time += Time.deltaTime;
                float t = time / moveDuration;

                // 線形補間（必要なら SmoothStep に変更可）
                transform.position = Vector3.Lerp(startPos, targetPos, t);

                yield return null;
            }
            transform.position = targetPos;
        }
        public void CreateOrder()
        {
            int itemA = itemPool[Random.Range(0, itemPool.Length)];
            int itemB = itemPool[Random.Range(0, itemPool.Length)];

            while (itemA == itemB)
            {
                itemB = itemPool[Random.Range(0, itemPool.Length)];
            }
            Debug.Log($"itemA:{itemA} itemB:{itemB}");

            // タプルで受け取る
            var (spriteA, spriteB) = itemManager.SetOrder(itemA, itemB);

            // UI 更新
            images[0].sprite = spriteA;
            images[1].sprite = spriteB;

            canvas.gameObject.SetActive(true);

            Debug.Log($"Customer が注文生成: {itemA}, {itemB}");
        }
    }
}