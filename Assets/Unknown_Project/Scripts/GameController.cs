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
        [SerializeField] private Animator idolAnim;
        [SerializeField] private Transform directLight;
        private int StairUpMotionReserveNum;
        private bool playingStairUpMotion;



        private void Start()
        {
            manager = FindFirstObjectByType<GameManager>();
            ctrlB = FindFirstObjectByType<ButtonController>();
            animeCtrl = FindFirstObjectByType<AnimationController>();


            time = 0.0;
            Time.timeScale = 0;
            deltaTime = 0.0;

            StairUpMotionReserveNum = 0;
            playingStairUpMotion = false;
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

                directLight.Rotate(levelData.sunSpeed,0,0);

                if(StairUpMotionReserveNum > 0 && !playingStairUpMotion)
                {
                    idolAnim.SetBool("IsStairUpping",true);
                    playingStairUpMotion = true;
                    StairUpMotionReserveNum--;
                }


                ctrlB.BCUpdate();
                animeCtrl.AnimetionUpdate(deltaTime);

            }
            else if(manager.GetGameState() == GameSceneState.Pause)
            {
                Time.timeScale = 0;
            
            }
            else if(manager.GetGameState() == GameSceneState.EndGame)
            {
                Time.timeScale = 1;
                if(ResultData.endTime == 0)ResultData.endTime = time;
                
                animeCtrl.AnimetionUpdate(deltaTime);

                if(StairUpMotionReserveNum > 0 && !playingStairUpMotion)
                {
                    idolAnim.SetBool("IsStairUpping",true);
                    playingStairUpMotion = true;
                    StairUpMotionReserveNum--;
                }
                if(StairUpMotionReserveNum <= 0 && !playingStairUpMotion)
                {
                    manager.finishAnimation = true;
                }


                if (manager.finishAnimation)
                {
                    manager.SetGameState(GameSceneState.Result);
                }
            }
            else if(manager.GetGameState() == GameSceneState.Result)
            {
                Time.timeScale = 1;

                
            }
            else
            {
                Debug.Log("Error : GameSceneState not found");  
            }

        }


        /////////////////////////////////////////
        /// 外部呼出し関数
        /////////////////////////////////////////
        public void StairUpMotionReservation()
        {
            StairUpMotionReserveNum++;
        }
        public void FinishStairUpMotion()
        {
            playingStairUpMotion = false;
        } 
    }
}
