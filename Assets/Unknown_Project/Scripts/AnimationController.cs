using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unknown_Project
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        private AnimationManager animeManager;
        [SerializeField] private GameObject stepClearParticlePrefab;

        private List<string> destroyAnimationNames = new List<string>{"StepClearAnimation"};
        private class AnimationSet
        {
            public string animeName;
            public List<AnimationManager> animes;
            public int playAnimeNum = 0;
        }

        private List<AnimationSet> playingAnimations;
        private List<AnimationSet> sampleMoveStockList;//右後から順番になっている（反転している）ことに注意, linear1/scaling1/linear2/scaling2の順だと思う...
        private List<int>delPlayingAnimeIndex;
        private bool SampleMoveAddFirst;


        private void Start()
        {
            animeManager = GetComponent<AnimationManager>();
    
        
            playingAnimations = new List<AnimationSet>();
            sampleMoveStockList = new List<AnimationSet>();
            delPlayingAnimeIndex = new List<int>();
            SampleMoveAddFirst = true;
        }





        public void AnimetionUpdate(double deltaTime)
        {
            for (int i = 0; i < playingAnimations.Count; i++)
            {
                if(playingAnimations[i] == null)
                {
                    delPlayingAnimeIndex.Add(i);
                    continue;
                }

                var currentAnimeData = playingAnimations[i];
                int playingAnimeNum = currentAnimeData.playAnimeNum;
                AnimationManager playingAnime = currentAnimeData.animes[playingAnimeNum];

                bool isEndAnimation = playingAnime.AnimationUpdate(deltaTime);

                if (isEndAnimation && playingAnimeNum < currentAnimeData.animes.Count - 1)
                {
                    currentAnimeData.playAnimeNum += 1;
                    //以降currentAnimeData関連の処理注意
                }
                else if (isEndAnimation)
                {
                    delPlayingAnimeIndex.Add(i);
                }
            }

            delPlayingAnimeIndex.Sort((a, b) => b.CompareTo(a));
            foreach(var delAnimeIndex in delPlayingAnimeIndex)
            {
                if (IsDestroy(playingAnimations[delAnimeIndex]))
                {
                    playingAnimations[delAnimeIndex].animes[0].DestroyObj();
                }
                if (IsChangeColor(playingAnimations[delAnimeIndex]))
                {
                    ChangeAlpha(playingAnimations[delAnimeIndex]);
                }
                playingAnimations.RemoveAt(delAnimeIndex);
            }
            delPlayingAnimeIndex = new List<int>();

        }



        private bool IsDestroy(AnimationSet set)
        {
            foreach(var animeName in destroyAnimationNames)
            {
                if(set.animeName == animeName)return true;
            }
            return false;
        }

        private bool IsChangeColor(AnimationSet set)
        {
            Vector3 scale = set.animes[0].GetObj().GetComponent<RectTransform>().localScale;

            if(set.animeName == "SampleMoveAnimation")
            {
                sampleMoveStockList.Add(set);
            }
            if(set.animeName == "SampleMoveAnimation" && scale == Vector3.one)return true;
            return false;            
        }

        private void ChangeAlpha(AnimationSet animeSet)
        {
            Image image = animeSet.animes[0].GetObj().GetComponent<Image>();
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }




        private LinearMoveEffect MakeLinearMoveAnimation(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
        {
            LinearMoveEffect lineAnime = obj.AddComponent<LinearMoveEffect>();
            lineAnime.AddProps(start,goal,obj,elapsedTime);

            return lineAnime;
        }

        private SmoothMoveEffect MakeSmoothMoveAnimation(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
        {
            SmoothMoveEffect smoothMoveEffect = obj.AddComponent<SmoothMoveEffect>();
            smoothMoveEffect.AddProps(start,goal,obj,elapsedTime);

            return smoothMoveEffect;
        }

        private ScalingEffect MakeScalingAnimation(Vector3 startScale, Vector3 goalScale, GameObject obj, double elapsedTime)
        {
            ScalingEffect scalingAnime = obj.AddComponent<ScalingEffect>();
            scalingAnime.AddProps(startScale, goalScale, obj, elapsedTime);

            return scalingAnime;
        }


        private void AddPlayingAnimes(List<AnimationManager> anime, string name)
        {
            AnimationSet set = new AnimationSet();
            set.animeName = name;
            set.animes = anime;
            playingAnimations.Add(set);
        }






        //////////////////////////////////////
        ///Add method
        //////////////////////////////////////

        public void AddLinearMoveAnimation(Vector2 goal, GameObject obj, double elapsedTime)
        {
            Vector2 start = obj.GetComponent<RectTransform>().anchoredPosition;
            AddLinearMoveAnimation(start, goal, obj, elapsedTime);
        }
        public void AddLinearMoveAnimation(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
        {
            List<AnimationManager> linearAnimations = new List<AnimationManager>();

            LinearMoveEffect linearMoveEffect = MakeLinearMoveAnimation(start, goal, obj, elapsedTime);
            linearAnimations.Add(linearMoveEffect);

            AddPlayingAnimes(linearAnimations, "LinearMove");
        }




        public void AddSmoothMoveAnimation(Vector2 goal, GameObject obj, double elapsedTime)
        {
            Vector2 start = obj.GetComponent<RectTransform>().anchoredPosition;
            AddSmoothMoveAnimation(start, goal, obj, elapsedTime);
        }
        public void AddSmoothMoveAnimation(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
        {
            List<AnimationManager> smoothAnimations = new List<AnimationManager>();

            SmoothMoveEffect smoothMoveEffect = MakeSmoothMoveAnimation(start, goal, obj, elapsedTime);
            smoothAnimations.Add(smoothMoveEffect);

            AddPlayingAnimes(smoothAnimations, "SmoothMove");
        }



        public void AddScalingAnimation(Vector3 goalScale, GameObject obj, double elapsedTime)
        {
            Vector3 startScale = obj.GetComponent<RectTransform>().localScale;
            AddScalingAnimation(startScale, goalScale, obj, elapsedTime);
        }
        public void AddScalingAnimation(Vector3 startScale, Vector3 goalScale, GameObject obj, double elapsedTime)
        {
            List<AnimationManager> scalingAnimations = new List<AnimationManager>();

            ScalingEffect scalingAnime = MakeScalingAnimation(startScale, goalScale, obj, elapsedTime);
            scalingAnimations.Add(scalingAnime);

            AddPlayingAnimes(scalingAnimations,"Scaling");
        }




        public void AddPopEffectAnimation(double scaleRate, GameObject obj, double elapsedTime)
        {
            List<AnimationManager> popAnimations = new List<AnimationManager>();

            Vector3 smallScale = Vector3.one * 0.9f * (float)scaleRate;
            Vector3 bigScale = Vector3.one * 1.05f * (float)scaleRate;
            Vector3 normalScale = Vector3.one * (float)scaleRate;

            ScalingEffect small = MakeScalingAnimation(smallScale, bigScale, obj, elapsedTime);
            popAnimations.Add(small);

            ScalingEffect big = MakeScalingAnimation(bigScale, normalScale, obj, elapsedTime);
            popAnimations.Add(big);

            AddPlayingAnimes(popAnimations, "Pop");
        }



        private const float LIFT_HEIGHT = 350;
        public void AddStepClearAnimation(GameObject[] objs)
        {
            foreach(var obj in objs)
            {
                List<AnimationManager> smoothMoveAnimations = new List<AnimationManager>();
                
                RectTransform rt = obj.GetComponent<RectTransform>();
                rt.localScale = rt.localScale/2;
                Vector2 start = rt.anchoredPosition;
                Vector2 temp = start;
                temp.y = LIFT_HEIGHT;
                temp.x += UnityEngine.Random.Range(-50f,50f);
                Vector2 goal = temp;

                SmoothMoveEffect smoothMoveEffect = MakeSmoothMoveAnimation(start,goal,obj,0.5f);
                smoothMoveAnimations.Add(smoothMoveEffect);

                ScalingEffect scalingEffect = MakeScalingAnimation(rt.localScale, Vector3.zero, obj, 0.05);
                smoothMoveAnimations.Add(scalingEffect);

                //時間経過用(particleと合わせる)
                LinearMoveEffect linearMove = MakeLinearMoveAnimation(goal, goal, obj, 0.45);
                smoothMoveAnimations.Add(linearMove);

                AddPlayingAnimes(smoothMoveAnimations, "StepClearAnimation");

                PlayParticle(obj);
            }
        }

        private void PlayParticle(GameObject obj)
        {
            GameObject objFx = Instantiate(stepClearParticlePrefab, obj.transform);
            objFx.transform.localPosition = Vector3.zero;
            objFx.transform.localRotation = Quaternion.identity;
            objFx.transform.localScale = Vector3.one;

        }



        private const double ELAPSED_TIME = 0.25;
        public void AddSampleMoveAnimation(double scaleRate, List<List<Vector2>>sampleButtonPos, Queue<List<GameObject>>sampleButtons)
        {
            if (SampleMoveAddFirst)
            {
                GenerateSampleMoveAnime(scaleRate, sampleButtonPos, sampleButtons);

                SampleMoveAddFirst = false;
            }
            else
            {
                for (int index = playingAnimations.Count-1; index >= 0; index--)
                {
                    if(playingAnimations[index].animeName == "SampleMoveAnimation")
                    {
                        sampleMoveStockList.Add(playingAnimations[index]);
                        playingAnimations.RemoveAt(index);
                    }
                }

                sampleMoveStockList.Reverse();

                int cnt=0;
                int i=0;
                foreach(var sampleRowButtons in sampleButtons)
                {
                    for(int j=0; j<levelData.samplePerRow; j++)
                    {
                        if(i != LevelData.SAMPLE_STEP_COUNT - 1)
                        {
                            sampleMoveStockList[cnt].animes[0].AddProps(sampleRowButtons[j],ELAPSED_TIME);
                            playingAnimations.Add(sampleMoveStockList[cnt]);

                            cnt++;

                            sampleMoveStockList[cnt].animes[0].AddProps(sampleRowButtons[j],ELAPSED_TIME);
                            playingAnimations.Add(sampleMoveStockList[cnt]);

                            cnt++;
                            
                        }
                        else
                        {
                            sampleMoveStockList[cnt].animes[0].AddProps(sampleRowButtons[j],ELAPSED_TIME);
                            playingAnimations.Add(sampleMoveStockList[cnt]);

                            sampleRowButtons[j].GetComponent<RectTransform>().anchoredPosition = sampleButtonPos[i][j];

                            cnt++;                            
                        }
                    }
                    i++;
                }
                sampleMoveStockList = new List<AnimationSet>();

            }
        }

        private void GenerateSampleMoveAnime(double scaleRate, List<List<Vector2>>sampleButtonPos, Queue<List<GameObject>>sampleButtons)
        {
            int i=0;
            foreach(var sampleRowButtons in sampleButtons)
            {
                for(int j=0; j<levelData.samplePerRow; j++)
                {
                    List<AnimationManager> linearAnimes = new List<AnimationManager>();
                    List<AnimationManager> scalingAnimes = new List<AnimationManager>();
                    if(i+1 < LevelData.SAMPLE_STEP_COUNT)
                    {
                        Vector2 goalPos = sampleButtonPos[i][j];
                        Vector2 startPos = sampleButtonPos[i+1][j];
                        GameObject sampleButton = sampleRowButtons[j];
                        Vector3 scale = Vector3.one;
                        Vector3 startScale = scale * (float)Math.Pow(scaleRate,i+1);
                        Vector3 goalScale = scale * (float)Math.Pow(scaleRate,i);
                        

                        LinearMoveEffect linear = MakeLinearMoveAnimation(startPos,goalPos,sampleButton,ELAPSED_TIME);
                        linearAnimes.Add(linear);
                        AddPlayingAnimes(linearAnimes,"SampleMoveAnimation");

                        
                        ScalingEffect scaling = MakeScalingAnimation(startScale,goalScale,sampleButton,ELAPSED_TIME);
                        scalingAnimes.Add(scaling);
                        AddPlayingAnimes(scalingAnimes,"SampleMoveAnimation");
                        
                    }
                    else
                    {
                        GameObject sampleButton = sampleRowButtons[j];
                        RectTransform rt = sampleButton.GetComponent<RectTransform>();
                        rt.anchoredPosition = sampleButtonPos[i][j];
                        Vector3 startScale = Vector3.zero;
                        rt.localScale = startScale;
                        Vector3 scale = Vector3.one;
                        Vector3 goalScale = scale * (float)Math.Pow(scaleRate,i);
                        

                        ScalingEffect scaling = MakeScalingAnimation(startScale,goalScale,sampleButton,ELAPSED_TIME);
                        scalingAnimes.Add(scaling);
                        AddPlayingAnimes(scalingAnimes,"SampleMoveAnimation");
                        
                        
                    }
                }
                i++;
            }
        }
    }
}
