using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown_Project
{
    public class AnimationController : MonoBehaviour
    {
        private AnimationManager animeManager;

        private List<(List<AnimationManager>,int)> playingAnimations;
        private List<int>delPlayingAnimeIndex;


        private void Start()
        {
            animeManager = GetComponent<AnimationManager>();
    
        
            playingAnimations = new List<(List<AnimationManager>,int)>();
            delPlayingAnimeIndex = new List<int>();
        }





        public void AnimetionUpdate(double deltaTime)
        {
            for (int cnt = 0; cnt < playingAnimations.Count; cnt++)
            {
                if(playingAnimations[cnt].Item1 == null)
                {
                    delPlayingAnimeIndex.Add(cnt);
                    continue;
                }
                var popAnime = playingAnimations[cnt];
                bool isEndAnimation = popAnime.Item1[popAnime.Item2].AnimationUpdate(deltaTime);

                if (isEndAnimation && popAnime.Item2 < popAnime.Item1.Count - 1)
                {
                    playingAnimations[cnt] = (popAnime.Item1, popAnime.Item2 + 1);
                }
                else if (isEndAnimation)
                {
                    delPlayingAnimeIndex.Add(cnt);
                }
            }

            foreach(var delPopAnimeIndex in delPlayingAnimeIndex)
            {
                playingAnimations.RemoveAt(delPopAnimeIndex);
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


        private void AddPlayingAnimes(List<AnimationManager> anime)
        {
            playingAnimations.Add((anime,0));
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

            AddPlayingAnimes(linearAnimations);
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

            AddPlayingAnimes(scalingAnimations);
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

            AddPlayingAnimes(popAnimations);
        }
    }
}
