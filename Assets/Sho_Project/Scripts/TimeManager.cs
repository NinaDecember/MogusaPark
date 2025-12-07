using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float gameTime = 60f;
    [SerializeField] private TextMeshProUGUI timerText;

    public float CurrentTime { get; private set; }
    public bool IsRunning { get; private set; } = false;

    public System.Action OnTimeUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTime = gameTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRunning) return;

        CurrentTime -= Time.deltaTime;
        if (CurrentTime < 0)
        {
            CurrentTime = 0;
            IsRunning = false;

            OnTimeUp?.Invoke();
        }

        // UIXV
        timerText.text = "Time:" + Mathf.Ceil(CurrentTime).ToString();
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        CurrentTime = gameTime;
        timerText.text = Mathf.Ceil(CurrentTime).ToString();
    }
}
