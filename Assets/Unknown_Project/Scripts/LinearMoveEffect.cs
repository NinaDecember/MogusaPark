using System.Collections.Generic;
using UnityEngine;

public class LinearMoveEffect : AnimationManager
{
    private Vector2 startPos;
    private Vector2 goalPos;
    private Vector2 vector;



    public override bool AnimationUpdate(double deltaTime)
    {
        rtObj.anchoredPosition += vector * (float)deltaTime;
        time += deltaTime;
        if(time >= elapsedTime)
        {
            rtObj.anchoredPosition = goalPos;
            return true;
        }

        return false;
    }


    public void AddProps(Vector2 start, Vector2 goal, GameObject obj, double elapsedTime)
    {
        base.AddProps(obj,elapsedTime);
        startPos = rtObj.anchoredPosition;
        goalPos = goal;
        time = 0f;
        elapsedTime = time;

        Vector2 distance = goal-startPos;
        vector = distance / (float)time;
    }


}
