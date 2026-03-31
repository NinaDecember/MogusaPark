using System.Collections;
using UnityEngine;

namespace Unknown_Project
{
    public class SceneSettingActivetor : MonoBehaviour
    {
        [SerializeField] private SceneSetting sceneSetting;
        private void Start()
        {

        }

        public IEnumerator ScreenSetting()
        {
            if(sceneSetting.screenAngle == ScreenAngle.Landscape)
            {
                if(Screen.orientation == ScreenOrientation.LandscapeRight)
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                else Screen.orientation = ScreenOrientation.LandscapeLeft;
                
                yield return null;
                
                Screen.orientation = ScreenOrientation.AutoRotation;

                Screen.autorotateToLandscapeLeft  = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait       = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }
            else if(sceneSetting.screenAngle == ScreenAngle.Portrait)
            {
                Screen.orientation = ScreenOrientation.Portrait;

                yield return null;

                Screen.orientation = ScreenOrientation.AutoRotation;

                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
            }
            else if(sceneSetting.screenAngle == ScreenAngle.Both)
            {
                Screen.orientation = ScreenOrientation.AutoRotation;

                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
            }

            Application.targetFrameRate = sceneSetting.fps;

        }
    }
}
