using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown_Project
{
    public class AnimationController : MonoBehaviour
    {
        private AnimationManager animeManager;
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
                }
                else if (isEndAnimation)
                {
                    delPlayingAnimeIndex.Add(i);
                }
                //以降currentAnimeData関連の処理禁止
            }

            foreach(var delAnimeIndex in delPlayingAnimeIndex)
            {
                playingAnimations.RemoveAt(delAnimeIndex);
            }
            delPlayingAnimeIndex = new List<int>();

        }




        private LinearMoveEffect MakeLinearMoveAnimation(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
        {
            LinearMoveEffect lineAnime = obj.AddComponent<LinearMoveEffect>();
            lineAnime.AddProps(start,goal,obj,elapsedTime);

            return lineAnime;
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
    }
}
