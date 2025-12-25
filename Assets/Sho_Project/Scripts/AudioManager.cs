using UnityEngine;

namespace Sho_Project
{


    public class AudioManager : MonoBehaviour
    {
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
    }
}
