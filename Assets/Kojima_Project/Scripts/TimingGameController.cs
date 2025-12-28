using UnityEngine;
using UnityEngine.UI;

namespace Koji
{
    public class TimingGameController : MonoBehaviour
    {
        [Header("UI")]
        public RectTransform bar;
        public RectTransform centerZone;
        public float speed = 400f;

        bool isPlaying = false;
        int direction = 1;
        float leftLimit;
        float rightLimit;

        void Update()
        {
            if (!isPlaying || bar == null) return;

            Vector2 pos = bar.anchoredPosition;
            pos.x += direction * speed * Time.deltaTime;
            bar.anchoredPosition = pos;
            Debug.Log("バーのポジション" + bar.anchoredPosition);

            if (pos.x >= rightLimit) direction = -1;
            if (pos.x <= leftLimit) direction = 1;
        }

        public void StartGame()
        {
            RectTransform panel = GetComponent<RectTransform>();
            float halfWidth = panel.rect.width / 2f;

            leftLimit = -halfWidth + 30f;
            rightLimit = halfWidth - 30f;

            isPlaying = true;
            direction = 1;
            bar.anchoredPosition = new Vector2(leftLimit, 0);
            gameObject.SetActive(true);
        }


        public void StopGame()
        {
            isPlaying = false;
        }

        public bool CheckSuccess()
        {
            if (bar == null || centerZone == null) return false;

            float barX = bar.anchoredPosition.x;
            float zoneX = centerZone.anchoredPosition.x;
            float halfW = centerZone.rect.width / 2f;

            return (barX > zoneX - halfW && barX < zoneX + halfW);
        }
    }

}