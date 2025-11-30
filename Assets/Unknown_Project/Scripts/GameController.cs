using UnityEngine;

public class GameController : MonoBehaviour
{
    private LevelData levelData;
    private GameManager manager;
    
    private double time;
    private double deltaTime;



    private void Start()
    {
        levelData = FindFirstObjectByType<LevelData>();
        manager = FindFirstObjectByType<GameManager>();


        time = 0.0;
        Time.timeScale = 0;
        deltaTime = 0.0;
    }


    private void Update()
    {

        if(manager.GetGameState() == GameSceneState.Load)
        {
            Time.timeScale = 0;
            manager.SetGameState(GameSceneState.CountDown);
        }
        else if(manager.GetGameState() == GameSceneState.CountDown)
        {
            Time.timeScale = 0;
            manager.SetGameState(GameSceneState.Playing);
        }
        else if(manager.GetGameState() == GameSceneState.Playing)
        {
            Time.timeScale = 1;

            deltaTime = Time.deltaTime;
            time += deltaTime;
            

        }
        else if(manager.GetGameState() == GameSceneState.Pause)
        {
            Time.timeScale = 0;
            
        }
        else if(manager.GetGameState() == GameSceneState.Result)
        {
            Time.timeScale = 0;
            
        }
        else
        {
            Debug.Log("Error : GameSceneState not found");  
        }

    }
}
