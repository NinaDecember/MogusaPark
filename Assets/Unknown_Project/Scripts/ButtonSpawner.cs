using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    private LevelData levelData;


    private void Start()
    {
        levelData = FindFirstObjectByType<LevelData>();
    }

    public List<Button> SpawnSampleButton()
    {
        int buttonCount = levelData.ButtonPrefabs.Count;
        List<Button> buttons = new List<Button>();
        for(int i=0; i<levelData.samplePerRow; i++)
        {
            int randomIndex = Random.Range(0,buttonCount);
            buttons.Add(levelData.ButtonPrefabs[randomIndex]);
        }
        return buttons;
    }

    public void SpawnSelectionButton()
    {
        
    }
}
