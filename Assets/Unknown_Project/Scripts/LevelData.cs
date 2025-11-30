using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelData", menuName = "MyGame/LevelData")]
public class LevelData : ScriptableObject
{
    public int CourseDistance = 10;
    public int samplePerRow = 4;//3or4



    public List<Button> ButtonPrefabs;


    private void OnEnable()
    {
        //自動でAssets/Recources/UKS404/ButtonPrefabs/のファイルを代入してくれる
        if (ButtonPrefabs == null || ButtonPrefabs.Count == 0)
        {
            ButtonPrefabs = new List<Button>();
            // Resources フォルダから初期Prefabをロードする場合
            Button prefab1 = Resources.Load<Button>("UKS404/ButtonPrefabs/Bird");
            Button prefab2 = Resources.Load<Button>("UKS404/ButtonPrefabs/Branch");
            Button prefab3 = Resources.Load<Button>("UKS404/ButtonPrefabs/DeadLeaves");
            Button prefab4 = Resources.Load<Button>("UKS404/ButtonPrefabs/FallenLeaves");
            Button prefab5 = Resources.Load<Button>("UKS404/ButtonPrefabs/Maple");
            Button prefab6 = Resources.Load<Button>("UKS404/ButtonPrefabs/Plum");
            Button prefab7 = Resources.Load<Button>("UKS404/ButtonPrefabs/Rock");
            Button prefab8 = Resources.Load<Button>("UKS404/ButtonPrefabs/Squirrel");
            
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
