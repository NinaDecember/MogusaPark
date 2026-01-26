using System.Collections;
using UnityEngine;
using random = UnityEngine.Random;

namespace Nave_Project
{
    public class CharaMove : MonoBehaviour
    {
        public float speed = 2.0f;
        private Vector3 targetPos; // 移動先ワールド座標
        private Vector3 moveDir;   // 正規化された移動方向ベクトル

        void Start()
        {
            Camera cam = Camera.main;

            // カメラ外のランダムな初期位置（Viewport座標）
            Vector3 randomViewportPos;
            float rand = random.value;
            if (rand < 0.25f)
                randomViewportPos = new Vector3(random.Range(-0.5f, -0.1f), random.Range(-0.5f, -0.1f), cam.nearClipPlane + 0.5f);
            else if (rand < 0.5f)
                randomViewportPos = new Vector3(random.Range(-0.5f, -0.1f), random.Range(1.1f, 1.5f), cam.nearClipPlane + 0.5f);
            else if (rand < 0.75f)
                randomViewportPos = new Vector3(random.Range(1.1f, 1.5f), random.Range(1.1f, 1.5f), cam.nearClipPlane + 0.5f);
            else
                randomViewportPos = new Vector3(random.Range(1.1f, 1.5f), random.Range(-0.5f, -0.1f), cam.nearClipPlane + 0.5f);

            // 初期位置をワールド座標に変換
            Vector3 startPos = cam.ViewportToWorldPoint(randomViewportPos);
            transform.position = startPos;

            // 移動先はカメラ中心を通る反対側のカメラ外
            Vector3 centerPos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane + 0.5f));
            Vector3 oppositeViewport = new Vector3(1 - randomViewportPos.x, 1 - randomViewportPos.y, cam.nearClipPlane + 0.5f);
            targetPos = cam.ViewportToWorldPoint(oppositeViewport);

            // 移動方向ベクトルを計算（正規化）
            moveDir = (targetPos - startPos).normalized;

            StartCoroutine(Moving());
        }

        IEnumerator Moving()
        {
            while (true)
            {
                transform.position += moveDir * speed * Time.deltaTime;

                // 目標位置に到達したら破棄
                if (Vector3.Dot(moveDir, targetPos - transform.position) <= 0f)
                {
                    Destroy(gameObject);
                    yield break;
                }

                yield return null;
            }
        }
    }
}
