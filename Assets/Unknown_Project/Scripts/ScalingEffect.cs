using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Unknown_Project
{



    public class ScalingEffect : AnimationManager
    {
        private Vector3 startScale;
        private Vector3 goalScale;
        private double speed;   // = 1/elapsedTime


        public override bool AnimationUpdate(double deltaTime)
        {
            if(obj == null)return true;

            time += Time.deltaTime * speed;
            rtObj.localScale = Vector3.Lerp(startScale, goalScale, (float)time);
            if(time >= 1f)
            {
                rtObj.localScale = goalScale;
                return true;
            }

            return false;
        }


        public void AddProps(Vector3 startScale, Vector3 goalScale, GameObject obj, double elapsedTime)
        {
            base.AddProps(obj,elapsedTime);
            this.startScale = startScale;
            this.goalScale = goalScale;
            this.speed = 1/elapsedTime;
        }
    }
}
