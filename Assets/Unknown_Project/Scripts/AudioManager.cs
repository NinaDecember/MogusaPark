using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
namespace Unknown_Project
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipData audioClipData;
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource SESource;    
        [SerializeField] private AudioSource VoiceSource;    
        [SerializeField] private AudioSource BGMSource;    

        private void Start()
        {
            OnMasterSliderChanged(0.0001f);
        }

        public void SetBgmVolume(float value, string name)
        {
            mixer.SetFloat(name, Mathf.Log10(value) * 20);
        }

        public void OnBgmSliderChanged(float value)
        {
            SetBgmVolume(value,"BGMVolume");
        }
        public void OnSESliderChanged(float value)
        {
            SetBgmVolume(value,"SEVolume");
        }
        public void OnVoiceSliderChanged(float value)
        {
            SetBgmVolume(value,"VoiceVolume");
        }
        public void OnMasterSliderChanged(float value)
        {
            SetBgmVolume(value,"MasterVolume");
        }

        public void PlaySE(string name)
        {
            AudioClip clip = audioClipData.GetSE(name);
            SESource.PlayOneShot(clip);
        }
        public void PlayVoice(string name)
        {
            VoiceSource.clip = audioClipData.GetVoice(name);
            VoiceSource.Play();
        }
        public void PlayBGM(string name)
        {
            BGMSource.clip = audioClipData.GetBGM(name);
            BGMSource.Play();
        }

        public void StopVoice()
        {
            VoiceSource.Pause();
        }
        public void ResumeVoice()
        {
            VoiceSource.UnPause();
        }


    }
}
