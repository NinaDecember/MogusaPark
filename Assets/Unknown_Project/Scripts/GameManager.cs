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
        private GameSceneState state;   //enum
        private int goalDistance;   //ゴールまでの段数
        public bool isSaveComplete = false;

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


        public void EndGame()
        {
            state = GameSceneState.Result;

            StartCoroutine(SaveDataTime());
        }


        public int GetGoalDistance()
        {
            return goalDistance;
        }

        public void Advance()
        {
            goalDistance -= 1;
            if(goalDistance == 0)
            {
                EndGame();
            }
        }




        private IEnumerator SaveDataTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if(isSaveComplete)break;
            }
            // Debug.Log("change Scene");
            // Debug.Log("time:"+ResultData.endTime);
            SceneManager.LoadScene("ResultScene");
        }

    }
}
