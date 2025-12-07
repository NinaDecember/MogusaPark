using TMPro;
using UnityEngine;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;//クリアかゲームオーバーかを表示
    [SerializeField] private TextMeshProUGUI scoreText;//最終スコアの表示
    [SerializeField] private TextMeshProUGUI customerText;//成功した顧客の数

    public void Init(bool isClear,int score,int customer)
    {
        resultText.text = isClear?"ゲームクリア":"ゲームオーバー";
        scoreText.text = $"スコア：{score}";
        customerText.text = $"成功顧客数：{customer}";
        resultPanel.SetActive(true);
    }

    public void GameClear()
    {

    }

    public void GameOver()
    {

    }
}
