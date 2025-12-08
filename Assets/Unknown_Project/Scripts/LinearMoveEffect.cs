using System.Collections.Generic;
using UnityEngine;

public class LinearMoveEffect : MonoBehaviour
{
    private class AnimationProperty2D
    {
        public RectTransform rtObj;
        public Vector2 goalPos;
        public double time;
        public double elapsedTime;
        public Vector2 vector;
    };

    List<AnimationProperty2D>animations;
    List<int>delAnimationsIndex;


    private void Start()
    {
        animations = new List<AnimationProperty2D>();
        delAnimationsIndex = new List<int>();
    }


    public void LineMoveUpdate(double deltaTime)
    {
        int cnt = 0;
        foreach(var anime in animations)
        {
            if(anime.time >= anime.elapsedTime)
            {
                anime.rtObj.anchoredPosition = anime.goalPos;
                delAnimationsIndex.Add(cnt);
                continue;
            }
            anime.rtObj.anchoredPosition += anime.vector * (float)deltaTime;
            anime.time += deltaTime;

            cnt++;

        }

        foreach(var delAnimeIndex in delAnimationsIndex)
        {
            animations.RemoveAt(delAnimeIndex);
        }
        delAnimationsIndex = new List<int>();
    }


    public void AddALinerMove2D(GameObject obj, Vector2 goal, double time)
    {
        AnimationProperty2D animeProp = new AnimationProperty2D();
        animeProp.rtObj = obj.GetComponent<RectTransform>();
        Vector2 start = animeProp.rtObj.anchoredPosition;
        animeProp.goalPos = goal;
        animeProp.time = 0f;
        animeProp.elapsedTime = time;

        Vector2 distance = goal-start;
        animeProp.vector = distance / (float)time;

        animations.Add(animeProp);
    }
}
