using UnityEngine;
namespace HabScene
{
    using System.Collections.Generic;


    [CreateAssetMenu(fileName = "RuntimeDisplaySetting", menuName = "Scriptable Objects/RuntimeDisplaySetting")]
    public class RuntimeDisplaySetting : ScriptableObject
    {
        public List<RuntimeDisplaySettingData> screenSettings = new List<RuntimeDisplaySettingData>();

        private Dictionary<string, RuntimeDisplaySettingData> screenSettingsDict;

        private void OnEnable()
        {
            BuildDictionary();
        }

        private void BuildDictionary()
        {
            screenSettingsDict = new Dictionary<string, RuntimeDisplaySettingData>();
            foreach (var data in screenSettings)
            {
                if (!screenSettingsDict.ContainsKey(data.distinationSceneName))
                    screenSettingsDict.Add(data.distinationSceneName, data);
            }
        }

        public RuntimeDisplaySettingData GetScreenSettings(string name)
        {
            if (screenSettingsDict == null)
                BuildDictionary();

            return screenSettingsDict.TryGetValue(name, out var settings) ? settings : null;
        }


    }
}
