// chatGPT大幅サポート
using UnityEngine;
namespace Unknown_Project
{
    /// <summary>
    /// 指定した秒数で start → goal へ移動し、
    /// ゴールに近づくほど減速してピタッと止まる演出用Effect
    /// </summary>
    public class SmoothMoveEffect : AnimationManager
    {
        private Vector2 start;   // 開始位置（anchoredPosition）
        private Vector2 goal;    // 終了位置（anchoredPosition）

        private float elapsed;



        public void AddProps(Vector2 startPos, Vector2 goalPos, GameObject obj, double elapsedTime)
        {
            base.AddProps(obj,elapsedTime);
            start = startPos;
            goal = goalPos;
            rtObj.anchoredPosition = start;
            elapsed = 0f;
        }

        public override bool AnimationUpdate(double deltaTime)
        {

            // 経過時間加算
            elapsed += Time.deltaTime;

            // 0〜1 に正規化（はみ出し防止）
            float t = Mathf.Clamp01(elapsed / (float)elapsedTime);

            // Ease Out（二次）：最初速く、最後ゆっくり
            float easedT = 1f - Mathf.Pow(1f - t, 2f);

            // 位置更新
            rtObj.anchoredPosition = Vector2.Lerp(start, goal, easedT);

            // 終了判定
            if (t >= 1f)
            {
                rtObj.anchoredPosition = goal;
                return true;
            }
            return false;
        }
    }
}
