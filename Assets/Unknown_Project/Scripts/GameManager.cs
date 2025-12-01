using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    private ButtonManager managerB;
    private GameSceneState state;   //enum
    private int goalDistance;   //ゴールまでの段数

    //0:ButtonMana:GenerateSampleButtonInit() 1:ButtonNaba:GenerateSelectionButtonInit()
    public List<bool> loadClear;

    private void Start()
    {
        managerB = FindFirstObjectByType<ButtonManager>();


        state = GameSceneState.Load;
        goalDistance = levelData.CourseDistance;

        loadClear  = new List<bool>{false,false};
        
    }

    public void LoadSampleButton()
    {
        managerB.GenerateSampleButtonInit();
        managerB.GenerateSelectionButtonInit();
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
