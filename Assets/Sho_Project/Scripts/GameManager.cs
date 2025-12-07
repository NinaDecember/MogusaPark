using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Managers")]
    public ScoreManager scoreManager;
    public ItemManager itemManager;
    public TimeManager timeManager;
    public TrustGaugeManager trustGaugeManager;
    public PlayerController playerController;
    public ResultUIManager resultUIManager;

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isGameClear = false;

    [SerializeField] private CustomerSpawner customerSpawner;
    private int succsessCustomer=0;

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    void Awake()
    {
        scoreText.text = $"Score:{score.ToString("00000")}";
    }

    void Start()
    {
        StartGame();

    }


    // ---------------------------
    // ゲーム開始
    // ---------------------------
    public void StartGame()
    {
        isGameOver = false;
        isGameClear = false;
        succsessCustomer = 0;
        scoreManager.ResetScore();
        timeManager.OnTimeUp = GameClear;
        trustGaugeManager.OnGameOver = GameOver;
        timeManager.StartTimer();
    }

    // ---------------------------
    // 正解時（Player + Idol の共同成功）
    // ItemManager から呼ばれる
    // ---------------------------
    public void OnOrderSuccess()
    {
        if (isGameOver || isGameClear) return;
        succsessCustomer++;
        scoreManager.AddScore(100);
        trustGaugeManager.AddGauge();
        ScoreChange();
        customerSpawner.OnCustomerFinished();
    }

    // ---------------------------
    // ミスした時
    // ---------------------------
    public void OnOrderFail()
    {
        if (isGameOver || isGameClear) return;
        customerSpawner.OnCustomerFinished();
        trustGaugeManager.SubGauge();
        //itemManager.PrepareNextOrder();
    }

    // ---------------------------
    // ゲームオーバー
    // ---------------------------
    public void GameOver()
    {
        isGameOver = true;
        timeManager.StopTimer();
        playerController.DisableController();
        resultUIManager.Init(isGameClear, score, succsessCustomer);
        Debug.Log("GAME OVER!");
    }

    public void GameClear()
    {
        isGameClear = true;
        timeManager.StopTimer();
        playerController.DisableController();
        resultUIManager.Init(isGameClear, score, succsessCustomer);
        Debug.Log("GAME CLEAR");
    }

    private void ScoreChange()
    {
        score = scoreManager.Score;
        scoreText.text = $"Score:{score.ToString("00000")}";
    }

    public void ChangeScene(string sceneName)
    {
        Debug.Log("シーン移動：" + sceneName);
        //SceneManager.LoadScene(sceneName);
    }


}
