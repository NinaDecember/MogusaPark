using System.Collections.Generic;


namespace Unknown_Project
{


    using UnityEngine;
    public class AnimationManager : MonoBehaviour
    {
        protected GameObject obj;
        protected RectTransform rtObj;
        protected double elapsedTime;
        protected double time;  //ScalingEffectでは実際にtime秒ではないので注意

        public virtual bool AnimationUpdate(double deltaTime)
        {
            return false;
        }

        public void AddProps(GameObject obj, double elapsedTime)
        {
            this.obj = obj;
            this.rtObj = this.obj.GetComponent<RectTransform>();
            this.elapsedTime = elapsedTime;
            this.time = 0.0;
        }
    }
}
