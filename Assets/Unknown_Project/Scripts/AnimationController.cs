using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private AnimationManager animeManager;

    private List<AnimationManager> animes;
    private List<int>delAnimationsIndex;
    private List<(List<AnimationManager>,int)> popEffectAnimations;
    private List<int>delPopAnimeIndex;


    private void Start()
    {
        animeManager = GetComponent<AnimationManager>();
    
        
        animes = new List<AnimationManager>();
        delAnimationsIndex = new List<int>();
        popEffectAnimations = new List<(List<AnimationManager>,int)>();
        delPopAnimeIndex = new List<int>();
    }





    public void AnimetionUpdate(double deltaTime)
    {
        int cnt = 0;
        foreach(var anime in animes)
        {
            if(anime == null)
            {
                delAnimationsIndex.Add(cnt);
                continue;
            }

            bool isEndAnimation = anime.AnimationUpdate(deltaTime);
            if(isEndAnimation)delAnimationsIndex.Add(cnt);
            cnt++;
        }


        foreach(var delAnimeIndex in delAnimationsIndex)
        {
            animes.RemoveAt(delAnimeIndex);
        }
        delAnimationsIndex = new List<int>();


        //special

        for (cnt = 0; cnt < popEffectAnimations.Count; cnt++)
        {
            if(popEffectAnimations[cnt].Item1 == null)
            {
                delPopAnimeIndex.Add(cnt);
                continue;
            }
            var popAnime = popEffectAnimations[cnt];
            bool isEndAnimation = popAnime.Item1[popAnime.Item2].AnimationUpdate(deltaTime);

            if (isEndAnimation && popAnime.Item2 < popAnime.Item1.Count - 1)
            {
                popEffectAnimations[cnt] = (popAnime.Item1, popAnime.Item2 + 1);
            }
            else if (isEndAnimation)
            {
                delPopAnimeIndex.Add(cnt);
            }
        }

        foreach(var delPopAnimeIndex in delPopAnimeIndex)
        {
            popEffectAnimations.RemoveAt(delPopAnimeIndex);
        }
        delPopAnimeIndex = new List<int>();


    }




    public void AddLinearMoveAnimation(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
    {
        LinearMoveEffect lineAnime = obj.AddComponent<LinearMoveEffect>();
        lineAnime.AddProps(start,goal,obj,elapsedTime);

        animes.Add(lineAnime);
    }
    public void AddLinearMoveAnimation(Vector2 goal, GameObject obj, double elapsedTime)
    {
        LinearMoveEffect lineAnime = obj.AddComponent<LinearMoveEffect>();
        Vector2 start = obj.GetComponent<RectTransform>().anchoredPosition;
        lineAnime.AddProps(start,goal,obj,elapsedTime);

        animes.Add(lineAnime);
    }



    public void AddScalingAnimation(Vector3 startScale, Vector3 goalScale, GameObject obj, double elapsedTime)
    {
        ScalingEffect scalingAnime = obj.AddComponent<ScalingEffect>();
        scalingAnime.AddProps(startScale, goalScale, obj, elapsedTime);

        animes.Add(scalingAnime);
    }

    public void AddPopEffectAnimation(double scaleRate, GameObject obj, double elapsedTime)
    {
        List<AnimationManager> popEffectAnimation = new List<AnimationManager>();

        Vector3 smallScale = Vector3.one * 0.9f * (float)scaleRate;
        Vector3 bigScale = Vector3.one * 1.05f * (float)scaleRate;
        Vector3 normalScale = Vector3.one * (float)scaleRate;

        ScalingEffect small = obj.AddComponent<ScalingEffect>();
        small.AddProps(smallScale, bigScale, obj, elapsedTime);

        popEffectAnimation.Add(small);


        ScalingEffect big = obj.AddComponent<ScalingEffect>();
        big.AddProps(bigScale, normalScale, obj, elapsedTime);

        popEffectAnimation.Add(big);


        popEffectAnimations.Add((popEffectAnimation,0));
    }
}
