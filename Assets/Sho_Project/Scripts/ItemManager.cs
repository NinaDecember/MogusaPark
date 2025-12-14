namespace Sho_Project
{
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
        [SerializeField] private CustomerSpawner customerSpawner;

        [SerializeField] private Sprite[] sprites;

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

            idolChoice = item;
            CheckCompletion();
        }

        public void PlayerPick(int item)
        {
            if (playerPicked) return;

            playerPicked = true;
            playerChoice = item;

            CheckCompletion();
        }

        private void CheckCompletion()
        {
            if (!idolPicked || !playerPicked) return;
            Debug.Log($"選んだのは：{playerChoice}と{idolChoice}");
            bool correct =
                (idolChoice == currentItemA && playerChoice == currentItemB) ||
                (idolChoice == currentItemB && playerChoice == currentItemA);

            if (correct)
            {
                Debug.Log("結果：成功");
                gameManager.OnOrderSuccess();
                //customerSpawner.OnCustomerFinished();

            }
            else
            {
                //customerSpawner.OnCustomerFinished();
                Debug.Log("結果：失敗");
                gameManager.OnOrderFail();

            }
        }
    }
}