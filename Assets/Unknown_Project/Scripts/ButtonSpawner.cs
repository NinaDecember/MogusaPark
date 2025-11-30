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

    public List<GameObject> SpawnSampleButton()
    {
        int buttonCount = levelData.ButtonPrefabs.Count;
        List<GameObject> buttons = new List<GameObject>();
        for(int i=0; i<levelData.samplePerRow; i++)
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
