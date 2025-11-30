using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelData levelData;
    private GameSceneState state;   //enum
    private int goalDistance;   //ゴールまでの段数

    private void Start()
    {
        levelData = FindFirstObjectByType<LevelData>();


        state = GameSceneState.Load;
        goalDistance = levelData.CourseDistance;
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
