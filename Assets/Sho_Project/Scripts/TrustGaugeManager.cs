using UnityEngine;
using UnityEngine.UI;

public class TrustGaugeManager : MonoBehaviour
{
    public System.Action OnGameOver;

    [SerializeField] private Slider gaugeSlider;   // ← trustGauge → gaugeSlider（UI名として自然）
    [SerializeField] private float startValue = 5; // ← startGaugeNum → startValue（初期値）
    [SerializeField] private float gainAmount = 1; // ← addNum → gainAmount（増える量）
    [SerializeField] private float lossAmount = 1; // ← subNum → lossAmount（減る量）

    private float currentValue;                    // ← currentGaugeNum → currentValue（現在値）

    void Start()
    {
        currentValue = startValue;
        gaugeSlider.value = currentValue;
    }

    public void AddGauge()
    {
        currentValue += gainAmount;
        currentValue = Mathf.Clamp(currentValue, 0, gaugeSlider.maxValue);
        UpdateGauge();
    }

    public void SubGauge()
    {
        currentValue -= lossAmount;
        currentValue = Mathf.Clamp(currentValue, 0, gaugeSlider.maxValue);

        if (currentValue <= 0)
        {
            OnGameOver?.Invoke();
        }

        UpdateGauge();
    }

    private void UpdateGauge()
    {
        gaugeSlider.value = currentValue;
    }
}
