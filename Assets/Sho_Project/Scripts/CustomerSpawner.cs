namespace Sho_Project
{
    using System.Collections.Generic;
    using UnityEngine;

    public class CustomerSpawner : MonoBehaviour
    {
        [Header("Customer Settings")]
        public GameObject customerPrefab;

        [Header("Spawn Points (5つ)")]
        public Vector3[] spawnPositions; // ← Transform から変更！

        private List<Customer> customers = new List<Customer>();

        [SerializeField] private ItemManager itemManager;

        void Start()
        {
            InitCustomers();
        }

        //最初に5人生成
        void InitCustomers()
        {
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                SpawnCustomerAt(i);
            }
            customers[0].CreateOrder();
        }

        //指定位置に生成
        void SpawnCustomerAt(int index)
        {
            Vector3 pos = spawnPositions[index];
            GameObject obj = Instantiate(customerPrefab, pos, Quaternion.identity);

            Customer c = obj.GetComponent<Customer>();
            c.Init(itemManager);
            customers.Insert(index, c);
        }

        // -------------------------------
        // 注文が完了したときに呼ばれる
        // -------------------------------
        public void OnCustomerFinished()
        {
            if (customers.Count == 0) return;

            // 1. 先頭の客を消す
            Customer first = customers[0];
            customers.RemoveAt(0);
            Destroy(first.gameObject);

            // 2. 残りの客を詰める（即移動）
            for (int i = 0; i < customers.Count; i++)
            {
                Vector3 targetPos = spawnPositions[i];
                customers[i].MoveTo(targetPos);
            }

            // 3. 一番後ろへ新しい客を追加
            SpawnCustomerAt(customers.Count);

            // 4. 最初の客の商品を表示させる
            customers[0].CreateOrder();
        }
    }
}