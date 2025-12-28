namespace Sho_Project
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using System.Collections;
    using System.Linq;

    public class PlayerController : MonoBehaviour
    {
        [Header("Move Points")]
        [SerializeField] private Vector3[] movePoints = new Vector3[3];
        private int index = 1;
        private bool isMoving = false;
        public float moveTime = 0.15f;

        [SerializeField] private Animator playerAnim;

        [Header("PickUp")]
        [SerializeField] private ItemManager itemManager;

        [Header("Input Actions")]
        [SerializeField] private InputAction moveLeftAction;
        [SerializeField] private InputAction moveRightAction;
        [SerializeField] private InputAction pickAction;

        private bool canControll = true;


        [Header("サウンド")]
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioData audioData;


        private void OnEnable()
        {
            moveLeftAction.performed += OnMoveLeft;
            moveRightAction.performed += OnMoveRight;
            pickAction.performed += OnPick;

            moveLeftAction.Enable();
            moveRightAction.Enable();
            pickAction.Enable();
        }

        private void OnDisable()
        {
            moveLeftAction.performed -= OnMoveLeft;
            moveRightAction.performed -= OnMoveRight;
            pickAction.performed -= OnPick;

            moveLeftAction.Disable();
            moveRightAction.Disable();
            pickAction.Disable();
        }

        public void DisableController()
        {
            canControll = false;
            moveLeftAction.Disable();
            moveRightAction.Disable();
            pickAction.Disable();

            Debug.Log("Player Controls Disabled");
        }

        public void EnableController()
        {
            canControll = true;
            moveLeftAction.Enable();
            moveRightAction.Enable();
            pickAction.Enable();
            Debug.Log("Player Controls Enabled");
        }

        private void Start()
        {
            transform.position = movePoints[1];
        }

        #region 移動関数
        private void OnMoveLeft(InputAction.CallbackContext ctx)
        {
            MoveRequest(-1);
        }

        private void OnMoveRight(InputAction.CallbackContext ctx)
        {
            MoveRequest(1);
        }

        public void MoveRequest(int dir)
        {
            if (isMoving) return;
            if (!canControll) return;
            audioManager.PlaySE(audioData.SEDatas.FirstOrDefault(name => name.name == "クリック音").clip);
            int targetIndex = index + dir;
            if (targetIndex < 0 || targetIndex >= movePoints.Length) return;

            index = targetIndex;
            StartCoroutine(SmoothMove(movePoints[index]));
        }

        private IEnumerator SmoothMove(Vector3 target)
        {
            isMoving = true;

            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < moveTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / moveTime;
                t = t * t * (3 - 2 * t); // スムーズステップ
                transform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }

            transform.position = target;
            isMoving = false;
        }
        #endregion

        #region アイテムピック関数
        private void OnPick(InputAction.CallbackContext ctx)
        {
            Debug.Log("PickUp:アイテムナンバー" + index);
            //アイテムマネージャーにアクセス
            itemManager.PlayerPick(index);
            audioManager.PlaySE(audioData.SEDatas.FirstOrDefault(name => name.name == "クリック音").clip);
            playerAnim.SetBool("IsLiftUp", true);
        }

        public void OnClickPick()
        {
            if (!canControll) return;
            itemManager.PlayerPick(index);
            audioManager.PlaySE(audioData.SEDatas.FirstOrDefault(name => name.name == "クリック音").clip);
            playerAnim.SetBool("IsLiftUp", true);
        }
        #endregion
    }
}