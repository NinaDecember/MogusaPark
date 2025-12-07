using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SnapEffect : MonoBehaviour
{
    private class AnimationSnapProperty2D
    {
        public RectTransform rtObj;
        public double time;
        public double elapsedTime;
        public Vector3 small = Vector3.one * 0.9f;
        public Vector3 big = Vector3.one * 1.05f;
        public Vector3 normal = Vector3.one;
        public int mode = 0;

    };
    private List<AnimationSnapProperty2D>animations;
    private List<int>delAnimationsIndex;

    private void Start()
    {
        animations = new List<AnimationSnapProperty2D>();
        delAnimationsIndex = new List<int>();
    }



    public void SnapUpdate(double deltaTime)
    {
        int cnt = 0;

        foreach(var anime in animations)
        {
            if(anime.rtObj == null)
            {
                cnt++;
                continue;
            }

            
            if(anime.mode == 0)
            {
                anime.time += Time.deltaTime * 15f;
                anime.rtObj.localScale = Vector3.Lerp(anime.small, anime.big, (float)anime.time);
                if(anime.time >= 1f){
                    anime.mode = 1;
                    anime.time = 0;
                }
            }
            else if(anime.mode == 1)
            {
                anime.time += Time.deltaTime * 12f;
                anime.rtObj.localScale = Vector3.Lerp(anime.big, anime.normal, (float)anime.time);
                if(anime.time >= 1f)
                {
                    anime.rtObj.localScale = anime.normal;

                    delAnimationsIndex.Add(cnt);
                }                
            }

            cnt++;
        }

        foreach(var delAnimeIndex in delAnimationsIndex)
        {
            animations.RemoveAt(delAnimeIndex);
        }
        delAnimationsIndex = new List<int>();
    }


    public void AddAnimationSnap2D(GameObject obj, double time)
    {
        AnimationSnapProperty2D animeProp = new AnimationSnapProperty2D();
        animeProp.rtObj = obj.GetComponent<RectTransform>();
        animeProp.time = 0f;
        animeProp.elapsedTime = time;
        animeProp.rtObj.localScale = animeProp.small;


        animations.Add(animeProp);
    }


}
