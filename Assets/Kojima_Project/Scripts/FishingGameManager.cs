using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Koji
{
    public class FishingGameManager : MonoBehaviour
    {
        [Header("Fishing Settings")]
        public Transform rodTip;
        public Transform lureHolder;
        public GameObject lurePrefab;
        public float castPower = 15f;

        [Header("Hit Settings")]
        public float minWaitTime = 1.5f;
        public float maxWaitTime = 4f;

        [Range(0f, 1f)]
        public float bigFishRate = 0.99f;   // ★ 大物率40%

        [Header("UI")]
        public Button fishButton;
        public TextMeshProUGUI resultText;      // Waiting / タイミング～用
        public TextMeshProUGUI resultPanelText;      // リザルト専用

        public GameObject resultPanel;

        [Header("Prologue")]
        public GameObject prologuePanel;      // 前芝居UI
        public TextMeshProUGUI prologueText;  // セリフ表示
        public Button prologueNextButton;     // 「釣りに行く」ボタン

        bool isPrologue = true;


        [Header("Timing Game")]
        public TimingGameController timingGame;
        public GameObject timingGamePanel;

        [Header("Fish Data")]
        public FishData[] fishes;

        [Header("Big Fish")]
        public GameObject bigFishPanel;
        public Slider mashGauge;
        public float mashRequired = 100f;
        public float mashPerTap = 10f;
        public float mashDecay = 15f;

        [Header("Result UI")]
        public Image resultFishImage;


        GameObject currentLure;
        FishData caughtFish;

        bool isFishing = false;
        bool isBigFishBattle = false;
        float currentMash = 0f;
        void Start()
        {
            fishButton.onClick.AddListener(OnFishButtonClick);

            // ★ ここが重要
            StartPrologue();

            SpawnLureAtRodTip();
        }

        void StartPrologue()
        {
            isPrologue = true;

            // 釣りボタンは無効
            fishButton.interactable = false;

            // プロローグUI表示
            prologuePanel.SetActive(true);

            prologueText.text =
                "アイドルがブローチを落としてしまった！\n" +
                "湖に落ちたみたい…\n\n" +
                "釣り上げてあげよう！";

            // 次へボタン
            prologueNextButton.onClick.RemoveAllListeners();
            prologueNextButton.onClick.AddListener(EndPrologue);
        }
        void EndPrologue()
        {
            isPrologue = false;

            prologuePanel.SetActive(false);

            // ★ ここで初めて釣り可能にする
            ResetState();
        }




        void SpawnLureAtRodTip()
        {
            if (currentLure != null) Destroy(currentLure);

            currentLure = Instantiate(lurePrefab, lureHolder.position, lureHolder.rotation);
            currentLure.transform.SetParent(lureHolder);
            currentLure.GetComponent<Rigidbody>().isKinematic = true;
        }

        void OnFishButtonClick()
        {
            // ★ プロローグ中は何もしない
            if (isPrologue) return;

            if (isBigFishBattle)
            {
                Mash();
                return;
            }

            if (timingGamePanel.activeSelf)
            {
                CheckTimingResult();
                return;
            }

            if (!isFishing)
            {
                Cast();
            }
        }



        void Cast()
        {
            isFishing = true;
            resultText.text = "魚を待つ...";

            currentLure.transform.SetParent(null);
            Rigidbody rb = currentLure.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            Vector3 dir = (rodTip.forward + Vector3.up * 0.5f).normalized;
            rb.AddForce(dir * castPower, ForceMode.Impulse);

            // ★ 大物じゃない時だけ無効化
            if (!isBigFishBattle)
                fishButton.interactable = false;

            StartCoroutine(FishingRoutine());
        }


        IEnumerator FishingRoutine()
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            Hit();
        }

        void Hit()
        {
            caughtFish = fishes[Random.Range(0, fishes.Length)];

            // 大物かどうかを確率＋データで判定
            bool isBig = caughtFish.isBig && Random.value < bigFishRate;

            if (isBig)
            {
                StartBigFishBattle();
            }
            else
            {
                StartTimingGame();
            }
        }


        void StartTimingGame()
        {
            timingGamePanel.SetActive(true);
            timingGame.StartGame();

            resultText.text = " タイミングを合わせろ！";
            fishButton.interactable = true;
        }
        void CheckTimingResult()
        {
            timingGame.StopGame();
            timingGamePanel.SetActive(false);
            fishButton.interactable = false;

            if (timingGame.CheckSuccess())
            {
                //resultText.text = $" {caughtFish.fishName} を釣った！";
                ReelUp();
            }
            else
            {
                resultText.text = "逃げられた…";
                StartCoroutine(HideResultAfterDelay(2f));
            }
        }




        void StartBigFishBattle()
        {
            isBigFishBattle = true;
            currentMash = 0f;

            bigFishPanel.SetActive(true);

            if (mashGauge != null)
                mashGauge.value = 0f;

            // ★ 超重要：連打できるようにする
            fishButton.interactable = true;

            resultText.text = " 大物だ！連打しろ！！";
            Debug.Log("Big Fish Battle START");
        }



        void Update()
        {
            if (!isBigFishBattle) return;
            if (mashGauge == null) return;

            currentMash -= mashDecay * Time.deltaTime;
            currentMash = Mathf.Max(0, currentMash);

            mashGauge.value = currentMash / mashRequired;
        }



        void Mash()
        {
            currentMash += mashPerTap;
            mashGauge.value = currentMash / mashRequired;

            if (currentMash >= mashRequired)
            {
                BigFishCaught();
            }
        }

        void BigFishCaught()
        {
            isBigFishBattle = false;
            bigFishPanel.SetActive(false);

            // resultText.text = $" 大物 {caughtFish.fishName} を釣り上げた！！";

            // ここでは一旦操作不可
            fishButton.interactable = false;

            ReelUp();
        }


        void ReelUp()
        {
            StartCoroutine(ReelUpMotion());
        }

        IEnumerator ReelUpMotion()
        {
            Rigidbody rb = currentLure.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            while (Vector3.Distance(currentLure.transform.position, lureHolder.position) > 0.1f)
            {
                currentLure.transform.position = Vector3.Lerp(
                    currentLure.transform.position,
                    lureHolder.position,
                    10f * Time.deltaTime
                );
                yield return null;
            }

            // ===== リザルト表示 =====
            resultPanel.SetActive(true);

            // テキスト
            if (resultText != null && caughtFish != null)
            {
                resultPanelText.text = $"{caughtFish.fishName} を釣った！";
                Debug.Log("[RESULT TEXT] " + resultPanelText.text);
            }

            // 画像
            // ===== リザルト画像 =====
            if (resultFishImage == null)
            {
                Debug.LogError("resultFishImage が未設定");
            }
            else if (caughtFish == null)
            {
                Debug.LogError("caughtFish が null");
            }
            else if (caughtFish.icon == null)
            {
                Debug.LogError($"FishData.icon が null: {caughtFish.fishName}");
            }
            else
            {
                resultFishImage.gameObject.SetActive(true);
                resultFishImage.sprite = caughtFish.icon;
                resultFishImage.color = Color.white;

                // サイズ強制（重要）
                resultFishImage.rectTransform.sizeDelta = new Vector2(200, 200);

                Debug.Log($"[IMAGE OK] {caughtFish.icon.name}");
            }



            // 2秒後に閉じる
            StartCoroutine(HideResultAfterDelay(2f));
        }



        IEnumerator HideResultAfterDelay(float sec)
        {
            yield return new WaitForSeconds(sec);

            // リザルトを一旦閉じる
            resultPanel.SetActive(false);

            // ★ エンディング判定はここ
            if (caughtFish != null && caughtFish.isEndingItem)
            {
                Debug.Log("ENDING ITEM CAUGHT → ADVへ遷移");
                SceneManager.LoadScene("EndingADV");
                yield break;
            }

            // 通常ルート
            SpawnLureAtRodTip();
            ResetState();
        }


        void ResetState()
        {
            isFishing = false;
            isBigFishBattle = false;
            resultText.text = "釣る！ボタンを押そう！";
            fishButton.interactable = true;
        }
    }

}

