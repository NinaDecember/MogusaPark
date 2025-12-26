using System.Collections;
using System.Collections.Generic;
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
        public bool finishResultSetting = false;

        //0:ButtonMana:GenerateSampleButtonInit() 1:ButtonNaba:GenerateSelectionButtonInit()
        public List<bool> loadClear;

        [SerializeField] private GameObject game2d;
        [SerializeField] private GameObject gameUI;
        [SerializeField] private GameObject resultCanvas;
        [SerializeField] private GameObject idol;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject mainCamera;


        private void Start()
        {
            Application.targetFrameRate = 30;
            
            managerB = FindFirstObjectByType<ButtonManager>();
            gameCtrl = FindFirstObjectByType<GameController>();


            state = GameSceneState.Load;
            goalDistance = levelData.CourseDistance;

            loadClear  = new List<bool>{false,false};

            finishResultSetting = false;
        
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

            StartCoroutine(FinishResultTransTime());
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




        private IEnumerator FinishResultTransTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if(gameCtrl.isFinishFade)break;
            }

            ResultSetting();
        }


        private void ResultSetting()
        {
            idol.GetComponent<Animator>().SetTrigger("IsResult");
            player.GetComponent<Animator>().SetTrigger("IsResult");
            game2d.SetActive(false);
            gameUI.SetActive(false);
            resultCanvas.SetActive(true);
            
            Transform idolTrans = idol.GetComponent<Transform>();
            Transform playerTrans = player.GetComponent<Transform>();
            Transform mainCamTrans = mainCamera.GetComponent<Transform>();
            Vector3 resultIdolPos = new Vector3(6.2f,3.27f,15.7f);
            Quaternion resultIdolRot = Quaternion.Euler(0f,-12f,0f);
            Vector3 resultPlayerPos = new Vector3(5.8f,3.3f,15.75f);
            Quaternion resultPlayerRot = Quaternion.Euler(0f,7.75f,0f);
            Vector3 mainCamPos = new Vector3(4.85f,4.4f,13.5f);
            Quaternion mainCamRot = Quaternion.Euler(-2.65f,21.8f,0.185f);

            playerTrans.SetParent(null);
            idolTrans.SetParent(null);
            idolTrans.localPosition = resultIdolPos;
            idolTrans.localRotation = resultIdolRot;
            playerTrans.localPosition = resultPlayerPos;
            playerTrans.localRotation = resultPlayerRot;
            mainCamTrans.position = mainCamPos;
            mainCamTrans.rotation = mainCamRot;

            finishResultSetting = true;
        }

        public void SelectButtonInteractable()
        {
            List<GameObject> selectableButtons = managerB.GetSelectableButtons();
            foreach(var button in selectableButtons)
            {
                Button buttonCompo = button.GetComponent<Button>();
                buttonCompo.interactable = true;
            }

        }
        public void SelectButtonInteractDisable()
        {
            List<GameObject> selectableButtons = managerB.GetSelectableButtons();
            foreach(var button in selectableButtons)
            {
                Button buttonCompo = button.GetComponent<Button>();
                buttonCompo.interactable = false;
            }

        }

        //中継

        public bool GetIsFinishFade()
        {
            return gameCtrl.isFinishFade;
        }
        public void SetIsFinishFade(bool flg)
        {
            gameCtrl.isFinishFade = flg;
        }

        //ButtonManager中継
        public void DrawVoiceTextBM(string type)
        {
            StartCoroutine(managerB.DrawVoiceText(type));
        }

    }
}
