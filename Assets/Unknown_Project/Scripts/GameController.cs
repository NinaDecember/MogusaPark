using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Unknown_Project
{



    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private ScreenTransition screenTransition;
        private GameManager manager;
        private ButtonController ctrlB;
        private AnimationController animeCtrl;
        private AudioManager audioManager;
        private PythonClient pythonClient;


    
        private double time;
        private double deltaTime;
        [SerializeField] private Animator idolAnim;
        [SerializeField] private Transform directLight;
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject resultTopButton;
        [SerializeField] private GameObject resultRetryButton;
        [SerializeField] private VoiceText voiceTextData;
        [SerializeField] private TextMeshProUGUI resultVoiceTextField;
        private int StairUpMotionReserveNum;
        private bool playingStairUpMotion;
        private bool isStartFadeFirst;
        public bool isFinishFade;
        private bool isStartVoicePlayFirst;
        private bool isResultAudioPlayFirst;
        private bool isResultTransFirst;
        private bool overDay;
        private float lastRotX;
        private bool isTextAnimating;
        private bool nextTextFlg;





        private void Start()
        {
            manager = FindFirstObjectByType<GameManager>();
            ctrlB = FindFirstObjectByType<ButtonController>();
            animeCtrl = FindFirstObjectByType<AnimationController>();
            audioManager = FindFirstObjectByType<AudioManager>();
            pythonClient = FindFirstObjectByType<PythonClient>();



            audioManager.PlayBGM("Main");
            time = 0.0;
            Time.timeScale = 0;
            deltaTime = 0.0;

            StairUpMotionReserveNum = 0;
            playingStairUpMotion = false;
            isStartFadeFirst = true;
            isFinishFade = false;
            isStartVoicePlayFirst = true;
            isResultAudioPlayFirst = true;
            isResultTransFirst = true;
            overDay = false;
            lastRotX = 0f;

            pauseButton.interactable = false;
            resultTopButton.SetActive(false);
            resultRetryButton.SetActive(false);
            isTextAnimating = false;
            nextTextFlg = false;

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
                Time.timeScale = 1;

                if (isStartFadeFirst)
                {
                    isStartFadeFirst = false;
                    isFinishFade = false;
                    screenTransition.StartFadeOut();
                }

                if (isStartVoicePlayFirst && isFinishFade)
                {
                    isStartVoicePlayFirst = false;
                    pauseButton.interactable = true;
                    StartCoroutine(StartCount());
                }
                
            }
            else if(manager.GetGameState() == GameSceneState.Playing)
            {
                // Debug.Log("Playing");
                Time.timeScale = 1;

                deltaTime = Time.deltaTime;
                time += deltaTime;

                directLight.Rotate(levelData.sunSpeed,0,0);
                // Debug.Log(directLight.localEulerAngles);
                if(directLight.localEulerAngles.x > 345)
                {
                    if(lastRotX != 0 && lastRotX < directLight.localEulerAngles.x)
                    {
                        overDay = true;
                    }
                    else if(lastRotX == 0)
                    {
                        lastRotX = directLight.localEulerAngles.x;
                    }
                }
                else
                {
                    lastRotX = 0f;
                }


                if(StairUpMotionReserveNum > 0 && !playingStairUpMotion)
                {
                    idolAnim.SetBool("IsStairUpping",true);
                    playingStairUpMotion = true;
                    StairUpMotionReserveNum--;
                }



                pythonClient.PCUpdate(deltaTime);
                ctrlB.BCUpdate();
                animeCtrl.AnimetionUpdate(deltaTime);

            }
            else if(manager.GetGameState() == GameSceneState.Pause)
            {
                Time.timeScale = 0;
            
            }
            else if(manager.GetGameState() == GameSceneState.EndGame)
            {
                pythonClient.EndGame();

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

                if (isStartFadeFirst && manager.finishAnimation)
                {
                    isStartFadeFirst = false;
                    isFinishFade = false;
                    pauseButton.interactable = false;
                    screenTransition.StartFadeIn();
                }
                if (isResultTransFirst && isFinishFade)
                {
                    isResultTransFirst = false;
                    manager.SetGameState(GameSceneState.Result);
                }
            }
            else if(manager.GetGameState() == GameSceneState.Result)
            {
                Time.timeScale = 1;

                if(manager.finishResultSetting)
                    PlayAudioAndFade();

            }
            else
            {
                Debug.Log("Error : GameSceneState not found");  
            }

        }


        private IEnumerator StartCount()
        {
            audioManager.PlayVoice("Start");
            manager.DrawVoiceTextBM("Start");

            yield return StartCoroutine(WaitForScaledSeconds(3f));
            
            manager.SetGameState(GameSceneState.Playing);
            isStartFadeFirst = true;
            isFinishFade = false;
            manager.SelectButtonInteractable();
        }


        private void PlayAudioAndFade()
        {
            if (isResultAudioPlayFirst)
            {
                isResultAudioPlayFirst = false;
                audioManager.PlayBGM("Result");
                audioManager.PlaySE("NatureWind");
                // audioManager.PlaySE("WindSound");

                StartCoroutine(PlayVoice());

                

            }
        }


        private IEnumerator WaitForScaledSeconds(float seconds)
        {
            float t = 0f;
            while (t < seconds)
            {
                if (Time.timeScale > 0f)
                    t += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator PlayVoice()
        {
            yield return StartCoroutine(FadeOut());

            resultTopButton.SetActive(true);
            resultRetryButton.SetActive(true);

            float x = directLight.localEulerAngles.x;

            if (overDay)
            {
                audioManager.PlayVoice("LaterDate");
                StartCoroutine(DrawResultVoiceText("LaterDate"));
            }
            else if(x > 10 && x < 90)
            {
                audioManager.PlayVoice("Day");
                StartCoroutine(DrawResultVoiceText("Day"));
            }
            else if(x <= 10 || x > 355)
            {
                audioManager.PlayVoice("Evening");
                StartCoroutine(DrawResultVoiceText("Evening"));
            }
            else if(x < 355 && x > 270)
            {
                audioManager.PlayVoice("Night");
                StartCoroutine(DrawResultVoiceText("Night"));
            }

        }
        private IEnumerator FadeOut()
        {
            isFinishFade = false;
            screenTransition.StartFadeOut();
            while (!isFinishFade)
            {
                yield return null;
            }
        }
        private IEnumerator DrawResultVoiceText(string type)
        {
            while (isTextAnimating)
            {
                nextTextFlg = true;
                yield return null;
            }
            nextTextFlg = false;

            isTextAnimating = true;
            resultVoiceTextField.text = "";
            string text = voiceTextData.GetVoiceText(type);
            if(text != null)
            {
                foreach (char c in text)
                {
                    resultVoiceTextField.text += c;
                    yield return new WaitForSeconds(0.05f);

                    if(nextTextFlg)break;
                }                
            }
            isTextAnimating = false;
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

        public void FinishFade()
        {
            isFinishFade = true;
        }
    }
}
