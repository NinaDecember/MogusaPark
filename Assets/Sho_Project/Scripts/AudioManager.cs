using UnityEngine;
using UnityEngine.Audio;

namespace Sho_Project
{


    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource bgm_Source;
        [SerializeField] private AudioSource se_Source;
        [SerializeField] private AudioSource voice_Source;
        
        public void PlayBGM(AudioClip clcip)
        {
            bgm_Source.clip = clcip;
            bgm_Source.Play();
        }

        public void StopBGM()
        {
            bgm_Source.Stop();
        }

        public void PlaySE(AudioClip clcip)
        {
            se_Source.PlayOneShot(clcip);
        }

        public void PlayVoice(AudioClip clcip)
        {
            voice_Source.PlayOneShot(clcip);
        }

        public void SetBgmVolume(float value, string name)
        {
            audioMixer.SetFloat(name, Mathf.Log10(value) * 20);
        }

        public void OnBgmSliderChanged(float value)
        {
            SetBgmVolume(value, "BGMVolume");
        }
        public void OnSESliderChanged(float value)
        {
            SetBgmVolume(value, "SEVolume");
        }
        public void OnVoiceSliderChanged(float value)
        {
            SetBgmVolume(value, "VoiceVolume");
        }
        public void OnMasterSliderChanged(float value)
        {
            SetBgmVolume(value, "MasterVolume");
        }
    }
}
