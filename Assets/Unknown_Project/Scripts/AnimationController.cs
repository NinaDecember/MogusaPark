using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown_Project
{
    public class AnimationController : MonoBehaviour
    {
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
        private List<int>delPlayingAnimeIndex;


        private void Start()
        {
            animeManager = GetComponent<AnimationManager>();
    
        
            playingAnimations = new List<AnimationSet>();
            delPlayingAnimeIndex = new List<int>();
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
    }
}
