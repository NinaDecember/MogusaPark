using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelData", menuName = "MyGame/LevelData")]
public class LevelData : ScriptableObject
{
    public const int SAMPLE_STEP_COUNT = 4;
    public const int SELECTION_COUNT = 8;
    public int CourseDistance = 10;
    public int samplePerRow = 4;//3or4



    public List<GameObject> ButtonPrefabs;


    private void OnEnable()
    {
        //自動でAssets/Recources/UKS404/ButtonPrefabs/のファイルを代入してくれる
        if (ButtonPrefabs == null || ButtonPrefabs.Count == 0)
        {
            ButtonPrefabs = new List<GameObject>();
            // Resources フォルダから初期Prefabをロードする場合
            GameObject prefab1 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/Bird");
            GameObject prefab2 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/Branch");
            GameObject prefab3 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/DeadLeaves");
            GameObject prefab4 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/FallenLeaves");
            GameObject prefab5 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/Maple");
            GameObject prefab6 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/Plum");
            GameObject prefab7 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/Rock");
            GameObject prefab8 = Resources.Load<GameObject>("UKS404/ButtonPrefabs/Squirrel");
            
            ButtonPrefabs.Add(prefab1);
            ButtonPrefabs.Add(prefab2);
            ButtonPrefabs.Add(prefab3);
            ButtonPrefabs.Add(prefab4);
            ButtonPrefabs.Add(prefab5);
            ButtonPrefabs.Add(prefab6);
            ButtonPrefabs.Add(prefab7);
            ButtonPrefabs.Add(prefab8);
        }
    }
}

// 便利
        // if(spawner == null)Debug.Log("is null");
        // else Debug.Log("not null");

