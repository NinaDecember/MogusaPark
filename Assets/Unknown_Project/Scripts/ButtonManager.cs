using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    private GameManager manager;
    private ButtonController ctrlB;
    private ButtonSpawner spawner;
    private Queue<List<GameObject>>sampleGenList;
    private List<GameObject> currentSelectButton;


    private void Start()
    {
        manager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<ButtonSpawner>();
        ctrlB = FindFirstObjectByType<ButtonController>();

        sampleGenList = new Queue<List<GameObject>>();
        currentSelectButton = new List<GameObject>();

    }










/// <summary>
/// Sample Button
/// </summary>


    public void GenerateSampleButtonInit()
    {
        for(int i=0; i<LevelData.SAMPLE_STEP_COUNT; i++)
        {
            List<GameObject> nowStepButtonPrefabs = spawner.SpawnSampleButton(levelData.samplePerRow);
            sampleGenList.Enqueue(nowStepButtonPrefabs);
        }
        ctrlB.ReDrawSample(sampleGenList);

        manager.loadClear[0] = true;
    }

    public void GenerateSampleButton()
    {
        List<GameObject> nowStepButtonPrefabs = spawner.SpawnSampleButton(levelData.samplePerRow);
        sampleGenList.Enqueue(nowStepButtonPrefabs);
        ctrlB.ReDrawSample(sampleGenList);
    }






    public void UpdateSampleButtons()
    {
        sampleGenList.Dequeue();
        GenerateSampleButton();
    }








/// <summary>
/// Selection Button
/// </summary>

    public void GenerateSelectionButtonInit()
    {
        
        currentSelectButton.AddRange(spawner.SpawnSelectButtonInit(LevelData.SELECTION_COUNT));
        
        ctrlB.ReDrawSelection(currentSelectButton);

        manager.loadClear[1] = true;
    }

    public void GenerateSelectionButton(int delIndex)
    {
        currentSelectButton.Add(spawner.SpawnSelectButton(delIndex));
        ctrlB.ReDrawSelection(currentSelectButton);
    }

    public void UpdateSelectButtons()
    {
        // currentSelectButton remove task
        int delIndex = 0;
        GenerateSelectionButton(delIndex);
    }

}
