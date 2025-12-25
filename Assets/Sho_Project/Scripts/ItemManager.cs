namespace Sho_Project
{
    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemManager : MonoBehaviour
    {
        public int currentItemA;
        public int currentItemB;

        [SerializeField] private bool idolPicked = false;
        private int idolChoice;
        [SerializeField] private bool playerPicked = false;
        private int playerChoice;
        [SerializeField] private GameManager gameManager;

        [SerializeField] private IdolController idolCon;
        [SerializeField] private PlayerController playerCon;
        [SerializeField] private CustomerSpawner customerSpawner;

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private EffectManager effectManager;

        [SerializeField] private List<GameObject> playerItems = new List<GameObject>();
        [SerializeField] private List<GameObject> IdolItems = new List<GameObject>();
        private Dictionary<GameObject,Vector3> ItemDic = new Dictionary<GameObject,Vector3>();//オブジェクトと初期位置
        private Dictionary<GameObject, Coroutine> liftCoroutines
    = new Dictionary<GameObject, Coroutine>();


        private GameObject currentPlayerItem;
        private GameObject currentIdolItem;

        [SerializeField] private float liftHeight = 0.5f;
        [SerializeField] private float liftTime = 0.15f;


        [Header("サウンド")]
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioData audioData;

        private void Start()
        {
            foreach (var item in playerItems)
            {
                if (item != null && !ItemDic.ContainsKey(item))
                {
                    ItemDic.Add(item, item.transform.localPosition);
                }
            }

            foreach (var item in IdolItems)
            {
                if (item != null && !ItemDic.ContainsKey(item))
                {
                    ItemDic.Add(item, item.transform.localPosition);
                }
            }
        }
        public (Sprite spriteA, Sprite spriteB) SetOrder(int a, int b)
        {
            Debug.Log("SetOrder 呼び出し: idolPicked を false に初期化します");

            currentItemA = a;
            currentItemB = b;

            idolPicked = false;
            playerPicked = false;

            idolCon.SetOrder(a, b);
            Debug.Log($"注文セット: {a}, {b}");

            return (sprites[a], sprites[b]);
        }


        public void IdolPick(int item)
        {

            Debug.Log($"IdolPick 呼び出し: 現在 idolPicked = {idolPicked}");

            if (idolPicked) return;

            idolPicked = true;
            Debug.Log($"idolPicked を true にしました");
            //Itemを持ち上げる
            LiftUpItem(item,false);
            idolChoice = item;
            CheckCompletion();
        }

        public void PlayerPick(int item)
        {
            if (playerPicked) return;

            playerPicked = true;
            playerChoice = item;
            LiftUpItem(item, true);
            CheckCompletion();
        }

        //アイテムを持ち上げる処理
        private void LiftUpItem(int _item,bool IsPlayer)
        {
            GameObject selectObj;
            if (IsPlayer)
            {
                selectObj = playerItems[_item];
                currentPlayerItem = selectObj;
            }
            else
            {
                selectObj = IdolItems[_item];
                currentIdolItem = selectObj;
            }

            //selectObjを持ち上げる処理
            if (liftCoroutines.TryGetValue(selectObj, out var running))
            {
                StopCoroutine(running);
            }

            Coroutine c = StartCoroutine(LiftCoroutine(selectObj));
            liftCoroutines[selectObj] = c;
        }

        private IEnumerator LiftCoroutine(GameObject target)
        {
            if (!ItemDic.TryGetValue(target, out Vector3 startPos))
            {
                Debug.LogError($"初期位置が登録されていません: {target.name}");
                yield break;
            }

            Vector3 endPos = startPos + Vector3.up * liftHeight;

            float elapsed = 0f;

            while (elapsed < liftTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / liftTime;
                t = t * t * (3f - 2f * t);

                target.transform.localPosition =
                    Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }

            target.transform.localPosition = endPos;
        }

        private void ResetItem(GameObject item)
        {
            // コルーチンを止める
            if (liftCoroutines.TryGetValue(item, out var running))
            {
                StopCoroutine(running);
                liftCoroutines.Remove(item);
            }

            // 元の位置に戻す
            if (ItemDic.TryGetValue(item, out var pos))
            {
                item.transform.localPosition = pos; 
                ParticleSystem ps = item.GetComponentInChildren<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play();
                }
            }
        }


        private void CheckCompletion()
        {
            if (!idolPicked || !playerPicked) return;
            Debug.Log($"選んだのは：{playerChoice}と{idolChoice}");
            bool correct =
                (idolChoice == currentItemA && playerChoice == currentItemB) ||
                (idolChoice == currentItemB && playerChoice == currentItemA);
            StartCoroutine(ResultEffect(correct));
        }


        private IEnumerator ResultEffect(bool isCurrect)
        {
            playerCon.DisableController();

            if (isCurrect)
            {
                Debug.Log("結果：成功");
                //エフェクト
                effectManager.StartEffect("成功エフェクト");
                audioManager.PlaySE(audioData.SEDatas.FirstOrDefault(name => name.name == "成功音").clip);
                yield return new WaitForSeconds(0.5f);
                ResetItem(currentPlayerItem);
                ResetItem(currentIdolItem);
                yield return new WaitForSeconds(0.3f);
                gameManager.OnOrderSuccess();
                
            }
            else
            {
                Debug.Log("結果：失敗");
                effectManager.StartEffect("失敗エフェクト");
                audioManager.PlaySE(audioData.SEDatas.FirstOrDefault(name => name.name == "失敗音").clip);
                yield return new WaitForSeconds(0.5f);
                ResetItem(currentPlayerItem);
                ResetItem(currentIdolItem);
                yield return new WaitForSeconds(0.3f);
                gameManager.OnOrderFail();
                
            }

            idolCon.gameObject.GetComponent<Animator>().SetBool("IsLiftUp", false);
            playerCon.gameObject.GetComponent<Animator>().SetBool("IsLiftUp", false);

            playerCon.EnableController();
        }
    }
}