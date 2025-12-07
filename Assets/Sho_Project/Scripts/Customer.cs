using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private ItemManager itemManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image[] images;
    [Header("Item Pool")]
    public int[] itemPool = new int[] { 0, 1, 2 };

    public void Init(ItemManager itemManager)
    {
        this.itemManager = itemManager;
    }
    // CustomerSpawner から呼ばれた時に移動
    public void MoveTo(Vector3 targetPos)
    {
        // とりあえず瞬間移動でもOK
        transform.position = targetPos;

    }
    public void CreateOrder()
    {
        int itemA = itemPool[Random.Range(0, itemPool.Length)];
        int itemB = itemPool[Random.Range(0, itemPool.Length)];

        while (itemA == itemB)
        {
            itemB = itemPool[Random.Range(0, itemPool.Length)];
        }
        Debug.Log($"itemA:{itemA} itemB:{itemB}");

        // タプルで受け取る
        var (spriteA, spriteB) = itemManager.SetOrder(itemA, itemB);

        // UI 更新
        images[0].sprite = spriteA;
        images[1].sprite = spriteB;

        canvas.gameObject.SetActive(true);

        Debug.Log($"Customer が注文生成: {itemA}, {itemB}");
    }
}
