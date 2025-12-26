using System.Collections.Generic;
using UnityEngine;
namespace Unknown_Project
{
    [CreateAssetMenu(fileName = "VoiceText", menuName = "Scriptable Objects/VoiceText")]
    public class VoiceText : ScriptableObject
    {
        public List<VoiceTextData> voiceTextData = new List<VoiceTextData>();

        private Dictionary<string, string> voiceTextDict;

        public string GetVoiceText(string type)
        {
            if (voiceTextDict == null)
                BuildDictionary();

            return voiceTextDict.TryGetValue(type, out var dialogue) ? dialogue : null;
        }

        private void OnEnable()
        {
            BuildDictionary();
        }

        private void BuildDictionary()
        {
            voiceTextDict = new Dictionary<string, string>();
            foreach (var data in voiceTextData)
            {
                if (!voiceTextDict.ContainsKey(data.name))
                    voiceTextDict.Add(data.name, data.dialogue);
            }

        }
    }
}
