using UnityEngine;
namespace Unknown_Project
{
    [CreateAssetMenu(fileName = "SceneSetting", menuName = "Scriptable Objects/SceneSetting")]
    public class SceneSetting : ScriptableObject
    {
        public ScreenAngle screenAngle = ScreenAngle.Landscape;
        public int fps = 30;
    }
}
