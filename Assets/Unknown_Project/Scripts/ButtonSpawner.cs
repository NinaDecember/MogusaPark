using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform canvasTrans;


    private void Start()
    {
    }

    public List<GameObject> SpawnButton(int generateCount)
    {
        List<GameObject> buttons = new List<GameObject>();
        int buttonCount = levelData.ButtonPrefabs.Count;
        for(int i=0; i<generateCount; i++)
        {
            int randomIndex = Random.Range(0,buttonCount);
            GameObject obj = Instantiate(levelData.ButtonPrefabs[randomIndex],canvasTrans);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(10000, 0); 
            buttons.Add(obj);
        }
        return buttons;
    }

    public void SpawnSelectionButton()
    {
        
    }

}
