using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Unknown_Project
{






    public class ButtonController : MonoBehaviour
    {
        private ButtonManager managerB;
        private AnimationController animeCtrl;
        private AudioManager audioManager;
        private PythonClient pythonClient;


        [SerializeField] private LevelData levelData;

        [SerializeField] private GameObject centorSampleBack;
        [SerializeField] private GameObject sampleSpacePrefab;
        [SerializeField] private Canvas canvas;
        private RectTransform rt;

        private double baseWidth;
        private double baseHeight;
        private Vector2 baseCenter;
        private struct SampleSpaceData
        {
            public double width;
            public double height;
            public Vector2 center;
        }

        private List<SampleSpaceData> sampleSpaceList;
        private List<List<Vector2>>sampleButtonPos;
        private List<GameObject>sampleBack;
        private List<Vector2> selectButtonPositions;


        private List<GameObject> activeButton;


        private void Start()
        {
            managerB = FindFirstObjectByType<ButtonManager>();
            animeCtrl = FindFirstObjectByType<AnimationController>();
            audioManager = FindFirstObjectByType<AudioManager>();
            pythonClient = FindFirstObjectByType<PythonClient>();


            rt = centorSampleBack.GetComponent<RectTransform>();
            baseCenter = rt.anchoredPosition;
            baseHeight = rt.rect.height;
            baseWidth = rt.rect.width;
            sampleSpaceList = CulcSampleSpaceData();
            sampleButtonPos = CulcSamplePositions();
            selectButtonPositions = CulcSelectButtonPositions();
            DrawSampleSpace();
            activeButton = new List<GameObject>();
        }


    /// <summary>
    /// 初期座標計算/初期処理
    /// </summary>


        private int SAMPLE_STEP_COUNT = LevelData.SAMPLE_STEP_COUNT;
        const double BASE_SCALE_RATE = 0.68;
        private List<SampleSpaceData> CulcSampleSpaceData()
        {
            List<SampleSpaceData> data = new List<SampleSpaceData>();

            //1行目だけ例外
            SampleSpaceData ssd1 = new SampleSpaceData();
            ssd1.width = baseWidth;
            ssd1.height = baseHeight;
            ssd1.center = baseCenter;
            data.Add(ssd1);
            for(int i=1; i<SAMPLE_STEP_COUNT; i++)
            {
                SampleSpaceData ssd = new SampleSpaceData();
                // ssd.width = data[i-1].width * Math.Pow(BASE_SCALE,i);
                // ssd.height = data[i-1].height * Math.Pow(BASE_SCALE,i);
                ssd.width = data[i-1].width * BASE_SCALE_RATE;
                ssd.height = data[i-1].height * BASE_SCALE_RATE;
            
                double centerX = baseCenter.x;
            
                double lastTopY;{
                    double lastCenterY = data[i-1].center.y;
                    double lastHeight = data[i-1].height;
                    lastTopY = lastCenterY + lastHeight/2;
                }
                double currentHeightHalf = ssd.height/2;
                double currentCentorY = lastTopY + currentHeightHalf;

                ssd.center = new Vector2((float)centerX,(float)currentCentorY);



                data.Add(ssd);
            
            }
            return data;
        }
        private List<List<Vector2>> CulcSamplePositions()
        {
            List<List<Vector2>> posList = new List<List<Vector2>>();

            for(int i=0; i<SAMPLE_STEP_COUNT; i++)
            {
                List<Vector2>rowPosList = new List<Vector2>();

                double x = 0;
                double y = sampleSpaceList[i].center.y;
                double interval = sampleSpaceList[i].width/levelData.samplePerRow;
                double leftSpace = interval/2;
                double leftX = -sampleSpaceList[i].width/2;
                for(int j=0; j<levelData.samplePerRow; j++)
                {
                    x = leftX + leftSpace + interval*j;
                    rowPosList.Add(new Vector2((float)x,(float)y));
                }

                posList.Add(rowPosList);
            }
            return posList;
        }

        private List<Vector2> CulcSelectButtonPositions()
        {
            int selectCount = LevelData.SELECTION_COUNT;
            const double RIGHT_X = 880;
            SampleSpaceData ssd = new SampleSpaceData();
            ssd.center = new Vector2(0,-300);//この位置固定
            ssd.width = RIGHT_X*2;
            ssd.height = ssd.center.y;


            List<Vector2> pos = new List<Vector2>();
            double interval = ssd.width/selectCount;
            double leftSpace = interval/2;
            double leftX = -ssd.width/2;

            double x = 0;
            double y = ssd.height;
            for(int i=0; i<selectCount; i++)
            {
                x = leftX + leftSpace + interval*i;
                pos.Add(new Vector2((float)x,(float)y));
            }

            return pos;
        }

        private void DrawSampleSpace()
        {
            sampleBack = new List<GameObject>();
            for(int i=LevelData.SAMPLE_STEP_COUNT-1; i>0; i--)
            {
                GameObject back = Instantiate(sampleSpacePrefab,canvas.GetComponent<Transform>());
                RectTransform rt = back.GetComponent<RectTransform>();
                rt.anchoredPosition = sampleSpaceList[i].center;    
                rt.localScale = Vector3.one *(float) Math.Pow(BASE_SCALE_RATE,i);
                sampleBack.Add(back);
            }

        }



    /// <summary>
    /// update series
    /// </summary>




        public void BCUpdate()//ButtonController:BC
        {
            if (pythonClient.response)
            {
                List<string> marksCopy;

                lock (pythonClient.markLock)
                {
                    pythonClient.response = false;
                    marksCopy = new List<string>(pythonClient.selectedMark);
                }

                AudioResponse(marksCopy);
            }

            

            List<int> delActiveObj = new List<int>();
            int cnt = 0;
            foreach(var obj in activeButton)
            {                                            
                RectTransform rt = obj.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(10,10);

                delActiveObj.Add(cnt);
                cnt++;
            }

            delActiveObj.Sort((a, b) => b.CompareTo(a));

            foreach(var index in delActiveObj)
            {


                bool isSetFinished = false;
                GameObject targetObj = null;
                int targetIndex = 0;
                if(managerB.GetGameState() != GameSceneState.EndGame)
                {
                    isSetFinished = managerB.TrySet(activeButton[index], out targetObj, out targetIndex);
                }

                if (isSetFinished)
                {
                    RectTransform rtSelect = activeButton[index].GetComponent<RectTransform>();
                    RectTransform rtTarget = targetObj.GetComponent<RectTransform>();
                    rtSelect.anchoredPosition = rtTarget.anchoredPosition;

                    managerB.AddSetButtonsList(targetIndex,activeButton[index]);

                    bool isStepClear = managerB.IsStepClear();

                    if (!isStepClear)
                    {
                        animeCtrl.AddPopEffectAnimation(1.0,activeButton[index],levelData.snapAnimeElapsedTime);
                        audioManager.PlaySE("Selected");
                    }
                    else
                    {
                        pythonClient.ClearConn(true);
                    }
                }
                else
                {
                    pythonClient.ClearConn(false);
                    Vector2 returnPos = activeButton[index].GetComponent<ButtonPressDetector>().GetReturnInitPos();
                    activeButton[index].GetComponent<RectTransform>().anchoredPosition = returnPos;
                    // animeCtrl.AddLinearMoveAnimation(returnPos, activeButton[index],levelData.lineAnimeElapsedTime);
                }
                activeButton.RemoveAt(index);
            }


        }

        private void TrackingPoint(GameObject obj)
        {
            Vector2 screenPos;

            if (Input.touchCount > 0)
            {
                screenPos = Input.GetTouch(0).position;
            }
            else
            {
                screenPos = Input.mousePosition;
            }

            Camera uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                screenPos,
                uiCam,
                out localPos
            );
            
            obj.GetComponent<RectTransform>().anchoredPosition = localPos;
        }




    /// <summary>
    ///  外部呼出し関数
    /// </summary>

        public Vector2 GetSampleButtonPos(int step, int col)
        {
            return sampleButtonPos[step][col];
        }

        public void ReDrawSampleButtons(Queue<List<GameObject>> buttons)
        {
            animeCtrl.AddSampleMoveAnimation(BASE_SCALE_RATE, sampleButtonPos, buttons);
        }


        public void ReDrawSelectionButtons(List<GameObject> generateSelectButtons)
        {
            int cnt = 0;
            foreach(var button in generateSelectButtons)
            {
                RectTransform rt = button.GetComponent<RectTransform>();
                rt.anchoredPosition = selectButtonPositions[cnt]-new Vector2(0,1000);
                button.GetComponent<ButtonPressDetector>().SetInitPos();

                Vector3 startScale = Vector3.zero;
                rt.localScale = startScale;
                Vector3 goalScale = Vector3.one;
                animeCtrl.AddScalingAnimation(startScale, goalScale, button, 0.1);

                cnt++;
            }
        }
        public void ReDrawSelectionButtons(int index, GameObject button)
        {
            RectTransform rt = button.GetComponent<RectTransform>();
            rt.anchoredPosition = selectButtonPositions[index]-new Vector2(0,1000);
            button.GetComponent<ButtonPressDetector>().SetInitPos();

            Vector3 startScale = Vector3.zero;
            rt.localScale = startScale;
            Vector3 goalScale = Vector3.one;
            animeCtrl.AddScalingAnimation(startScale, goalScale, button, 0.1);

        }


        public void Pushed(GameObject button)
        {
            if(button.GetComponent<Button>().interactable){
                activeButton.Add(button);
                button.transform.SetAsLastSibling();
                audioManager.PlaySE("Click");
            }
        }

        public void AudioResponse(List<string> marks)
        {
            List<GameObject>buttons = managerB.GetSelectableButtons();
            // Debug.Log("marks:"+marks[0]+", "+marks[1]+", "+marks[2]);

            foreach(var mark in marks)
            {
                foreach(var buttonObj in buttons)
                {
                    if (buttonObj == null)
                    {
                        Debug.LogError("buttonObj is null");
                        continue;
                    }

                    if(buttonObj.tag == mark)
                    {
                        GameObject button = buttonObj;
                        if(button.GetComponent<Button>().interactable){
                            activeButton.Add(button);
                            button.transform.SetAsLastSibling();
                            // audioManager.PlaySE("Click");
                        }
                    }
                }
            }
        }

    }
}
