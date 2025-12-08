using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
        moveLeftAction.Disable();
        moveRightAction.Disable();
        pickAction.Disable();

        Debug.Log("Player Controls Disabled");
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

    private void MoveRequest(int dir)
    {
        if (isMoving) return;

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
        Debug.Log("PickUp:アイテムナンバー"+index);
        //アイテムマネージャーにアクセス
        itemManager.PlayerPick(index);
        playerAnim.SetBool("IsLiftUp", true);
    }
    #endregion
}
