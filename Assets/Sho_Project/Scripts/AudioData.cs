namespace Sho_Project
{

    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
    public class AudioData : ScriptableObject
    {
        public List<Data> BGMDatas = new List<Data>();
        public List<Data> SEDatas = new List<Data>();
        public List<Data> VoiceDatas = new List<Data>();
    }

    [System.Serializable]
    public class Data
    {
        public string name;
        public AudioClip clip;
    }

}