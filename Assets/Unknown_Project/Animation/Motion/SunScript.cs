using UnityEngine;

public class SunScript : MonoBehaviour
{
    float x;

    enum TimeOfDay
    {
        Morning,
        Noon,
        Night
    }

    TimeOfDay currentTime;

    bool dayStarted; // 0°通過検知用

    void Start()
    {
        currentTime = TimeOfDay.Night;
        dayStarted = false;
    }

    void Update()
    {
        // 太陽を回す
        x += Time.deltaTime * 10f;
        x %= 360f;

        transform.rotation = Quaternion.Euler(x, 0f, 0f);

        // --- 0°になったら Dayスタート ---
        if (x < 1f && !dayStarted)
        {
            dayStarted = true;
            Debug.Log("Dayスタート（0°通過）");
        }

        // 0°から離れたら次回に備えてリセット
        if (x > 5f)
        {
            dayStarted = false;
        }

        // --- 時間帯判定（0〜360） ---
        TimeOfDay newTime;

        if (x >= 5f && x < 110f)
        {
            newTime = TimeOfDay.Morning;
        }
        else if (x >= 110f && x < 145f)
        {
            newTime = TimeOfDay.Noon;
        }
        else
        {
            newTime = TimeOfDay.Night;
        }

        // 切り替わった瞬間だけデバッグ
        if (newTime != currentTime)
        {
            currentTime = newTime;
            Debug.Log("時間帯切り替え → " + currentTime + "（角度：" + x + "）");
        }
    }
}
