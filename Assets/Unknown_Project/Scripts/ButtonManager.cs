using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private LevelData levelData;
    private GameManager manager;
    private ButtonSpawner spawner;
    private Queue<List<Button>>sampleList;


    private void Start()
    {
        levelData = FindFirstObjectByType<LevelData>();
        manager = FindFirstObjectByType<GameManager>();
        spawner = FindFirstObjectByType<ButtonSpawner>();

        sampleList = new Queue<List<Button>>();
    }

    public void AddSampleButton()
    {
        List<Button> nowStepButton = spawner.SpawnSampleButton();
        sampleList.Enqueue(nowStepButton);
    }
}
