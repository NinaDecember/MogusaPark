using UnityEngine;

namespace Unknown_Project
{
    public class SceneSettingActivetor : MonoBehaviour
    {
        [SerializeField] private SceneSetting sceneSetting;
        void Start()
        {
            if(sceneSetting.screenAngle == ScreenAngle.Landscape)
            {
                Screen.orientation = ScreenOrientation.AutoRotation;

                Screen.autorotateToLandscapeLeft  = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait       = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }
            else if(sceneSetting.screenAngle == ScreenAngle.Portrait)
            {
                Screen.orientation = ScreenOrientation.Portrait;

                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
            }

            Application.targetFrameRate = sceneSetting.fps;
    

        }
    }
}
