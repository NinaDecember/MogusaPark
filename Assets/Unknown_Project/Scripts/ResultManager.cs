using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Image day;
    [SerializeField] private Image evening;
    [SerializeField] private Image night;
    private double time;

    private void Start()
    {
        time = ResultData.endTime;
        ResultData.Reset();
    }
}
