using UnityEngine;
namespace Koji
{
    [System.Serializable]
    public class FishData
    {
        public string fishName = "サバ";
        public bool isBig;
        public Sprite icon;   // ★ 追加
        [Range(1, 3)]
        public int rarity = 1;   // 1=普通, 2=レア, 3=激レア
                                 // ★追加
        public bool isEndingItem;
    }
}
