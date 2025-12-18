using UnityEngine;


namespace Unknown_Project
{



    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        private GameManager manager;
        private ButtonController ctrlB;
        private AnimationController animeCtrl;
    
        private double time;
        private double deltaTime;



        private void Start()
        {
            manager = FindFirstObjectByType<GameManager>();
            ctrlB = FindFirstObjectByType<ButtonController>();
            animeCtrl = FindFirstObjectByType<AnimationController>();


            time = 0.0;
            Time.timeScale = 0;
            deltaTime = 0.0;
        }

        private bool loadStart = false;
        private void Update()
        {

            if(manager.GetGameState() == GameSceneState.Load)
            {
                Time.timeScale = 0;
                if (!loadStart)
                {
                    loadStart = true;
                    manager.LoadSampleButton();
                }
                foreach(bool flg in manager.loadClear)
                {
                    if(!flg)return;
                }
                manager.SetGameState(GameSceneState.CountDown);
            }
            else if(manager.GetGameState() == GameSceneState.CountDown)
            {
                Time.timeScale = 0;
                manager.SetGameState(GameSceneState.Playing);
            }
            else if(manager.GetGameState() == GameSceneState.Playing)
            {
                // Debug.Log("Playing");
                Time.timeScale = 1;

                deltaTime = Time.deltaTime;
                time += deltaTime;

                ctrlB.BCUpdate();
                animeCtrl.AnimetionUpdate(deltaTime);

            }
            else if(manager.GetGameState() == GameSceneState.Pause)
            {
                Time.timeScale = 0;
            
            }
            else if(manager.GetGameState() == GameSceneState.Result)
            {
                Time.timeScale = 1;
                ResultData.endTime = time;
                manager.isSaveComplete = true;
            }
            else
            {
                Debug.Log("Error : GameSceneState not found");  
            }

        }
    }
}
