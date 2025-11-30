using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    private ButtonManager managerB;
    private GameSceneState state;   //enum
    private int goalDistance;   //ゴールまでの段数

    //0:ButtonMana:GenerateSampleButtonInit()
    public List<bool> loadClear = new List<bool>{false};

    private void Start()
    {
        managerB = FindFirstObjectByType<ButtonManager>();


        state = GameSceneState.Load;
        goalDistance = levelData.CourseDistance;

        
    }

    public void LoadSampleButton()
    {
        managerB.GenerateSampleButtonInit();
    }

    public void SetGameState(GameSceneState state)
    {
        this.state = state;
    }

    public GameSceneState GetGameState()
    {
        return state;
    }

}
