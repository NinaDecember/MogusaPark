namespace Sho_Project
{


    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
    public class EffectData : ScriptableObject
    {
        public List<Effect> effectDatas = new List<Effect>();
    }

    [Serializable]
    public class Effect
    {
        public string name;
        public GameObject effectObj;
        public Vector3 pos;
    }
}