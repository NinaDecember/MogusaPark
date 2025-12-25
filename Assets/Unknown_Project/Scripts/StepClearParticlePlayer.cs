using System.Collections;
using UnityEngine;

namespace Unknown_Project
{
    public class StepClearParticlePlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem glow;
        [SerializeField] private ParticleSystem brust;

        void Start()
        {
            StartCoroutine(ParticlePlaying());
        }


        private IEnumerator ParticlePlaying()
        {
            brust.Stop();
            glow.Play();
            yield return new WaitForSeconds(0.5f);

            glow.Stop();
            brust.Play();
            yield return new WaitForSeconds(1f);

            brust.Stop();
            Destroy(gameObject);
        }
    }
}
