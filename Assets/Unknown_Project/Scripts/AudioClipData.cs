using System.Collections.Generic;
using UnityEngine;
namespace Unknown_Project
{
    [CreateAssetMenu(fileName = "AudioClipData", menuName = "Scriptable Objects/AudioClipData")]
    public class AudioClipData : ScriptableObject
    {
        
        public List<AudioData> seData = new List<AudioData>();
        public List<AudioData> voiceData = new List<AudioData>();
        public List<AudioData> bgmData = new List<AudioData>();

        private Dictionary<string, AudioClip> seDict;
        private Dictionary<string, AudioClip> voiceDict;
        private Dictionary<string, AudioClip> bgmDict;

        public AudioClip GetSE(string name)
        {
            if (seDict == null)
                BuildDictionary();

            return seDict.TryGetValue(name, out var clip) ? clip : null;
        }
        public AudioClip GetVoice(string name)
        {
            if (voiceDict == null)
                BuildDictionary();

            return voiceDict.TryGetValue(name, out var clip) ? clip : null;
        }
        public AudioClip GetBGM(string name)
        {
            if (bgmDict == null)
                BuildDictionary();

            return bgmDict.TryGetValue(name, out var clip) ? clip : null;
        }

        private void OnEnable()
        {
            BuildDictionary();
        }

        private void BuildDictionary()
        {
            seDict = new Dictionary<string, AudioClip>();
            foreach (var data in seData)
            {
                if (!seDict.ContainsKey(data.name))
                    seDict.Add(data.name, data.clip);
            }

            voiceDict = new Dictionary<string, AudioClip>();
            foreach (var data in voiceData)
            {
                if (!voiceDict.ContainsKey(data.name))
                    voiceDict.Add(data.name, data.clip);
            }

            bgmDict = new Dictionary<string, AudioClip>();
            foreach (var data in bgmData)
            {
                if (!bgmDict.ContainsKey(data.name))
                    bgmDict.Add(data.name, data.clip);
            }
        }

    }
}
