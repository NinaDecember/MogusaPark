using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Unknown_Project
{



    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        private ButtonManager managerB;
        private GameController gameCtrl;
        private GameSceneState state;   //enum
        private int goalDistance;   //ゴールまでの段数
        public bool finishAnimation = false;

        //0:ButtonMana:GenerateSampleButtonInit() 1:ButtonNaba:GenerateSelectionButtonInit()
        public List<bool> loadClear;

        [SerializeField] private GameObject gameCanvas;
        [SerializeField] private GameObject idol;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject mainCamera;


        private void Start()
        {
            managerB = FindFirstObjectByType<ButtonManager>();
            gameCtrl = FindFirstObjectByType<GameController>();


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


        public void EndGame()
        {
            state = GameSceneState.EndGame;

            StartCoroutine(FinishedAnimationTime());
        }


        public int GetGoalDistance()
        {
            return goalDistance;
        }

        public void Advance()
        {
            gameCtrl.StairUpMotionReservation();
            goalDistance -= 1;
            if(goalDistance == 0)
            {
                EndGame();
            }
        }




        private IEnumerator FinishedAnimationTime()
        {
            Debug.Log("FinishAnimationTime");
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if(finishAnimation)break;
            }
            Debug.Log("woop exit"+finishAnimation);

            ResultSetting();
        }


        private void ResultSetting()
        {
            Debug.Log("ResultSetting");
            idol.GetComponent<Animator>().SetTrigger("IsResult");
            player.GetComponent<Animator>().SetTrigger("IsResult");
            gameCanvas.SetActive(false);
            
            Transform idolTrans = idol.GetComponent<Transform>();
            Transform playerTrans = player.GetComponent<Transform>();
            Transform mainCamTrans = mainCamera.GetComponent<Transform>();
            Vector3 resultIdolPos = new Vector3(6.2f,-0.6f,9.5f);
            Quaternion resultIdolRot = Quaternion.Euler(0f,-38f,0f);
            Vector3 resultPlayerPos = new Vector3(-0.005f,-0.00656f,0.00336f);
            Quaternion resultPlayerRot = Quaternion.Euler(-3.17f,62f,-6f);
            Vector3 mainCamPos = new Vector3(4.85f,4.4f,13.5f);
            Quaternion mainCamRot = Quaternion.Euler(-2.65f,21.8f,0.185f);

            idolTrans.localPosition = resultIdolPos;
            idolTrans.localRotation = resultIdolRot;
            playerTrans.localPosition = resultPlayerPos;
            playerTrans.localRotation = resultPlayerRot;
            mainCamTrans.position = mainCamPos;
            mainCamTrans.rotation = mainCamRot;
        }

    }
}
//y-5