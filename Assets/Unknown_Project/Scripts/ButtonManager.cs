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


    private void Start()
    {
        manager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<ButtonSpawner>();
        ctrlB = FindFirstObjectByType<ButtonController>();

        sampleGenList = new Queue<List<GameObject>>();

    }


    public void GenerateSampleButtonInit()
    {
        for(int i=0; i<LevelData.SAMPLE_STEP_COUNT; i++)
        {
            List<GameObject> nowStepButtonPrefabs = spawner.SpawnSampleButton();
            sampleGenList.Enqueue(nowStepButtonPrefabs);
        }
        ctrlB.ReDrawSample(sampleGenList);

        manager.loadClear[0] = true;
    }

    public void GenerateSampleButton()
    {
        List<GameObject> nowStepButtonPrefabs = spawner.SpawnSampleButton();
        sampleGenList.Enqueue(nowStepButtonPrefabs);
        ctrlB.ReDrawSample(sampleGenList);
    }


    public void UpdateButtons()
    {
        sampleGenList.Dequeue();
        GenerateSampleButton();
    }

}
