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
            
        }

        public void SetBgmVolume(float value)
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        }

        public void OnBgmSliderChanged(float value)
        {
            SetBgmVolume(value);
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


    }
}
