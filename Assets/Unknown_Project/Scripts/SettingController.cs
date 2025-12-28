using Sho_Project;
using UnityEngine;
namespace Unknown_Project
{
    public class SettingController : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private GameObject settingsScreen;
        [SerializeField] private Light directLight;


        private void Start()
        {
            settingsScreen.SetActive(false);
        }
        public void OnClickExitButton()
        {
            audioManager.PlaySE("Click");
            settingsScreen.SetActive(false);
        }

        public void OnClickShadowingButton(bool isOn)
        {
            if (isOn)
            {
                directLight.shadows = LightShadows.Soft;
            }
            else if (!isOn)
            {
                directLight.shadows = LightShadows.None;
            }
        }

    }
}
