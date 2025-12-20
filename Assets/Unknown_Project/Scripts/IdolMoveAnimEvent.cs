using System.Collections;
using UnityEngine;
namespace Unknown_Project
{
    public class IdolMoveAnimEvent : MonoBehaviour
    {
        [SerializeField] private Animator humanBaseAnimator;
        [SerializeField] private GameController gameController;
        public void StarthUpStairMoving()
        {
            humanBaseAnimator.SetTrigger("IsMove");
        }
        public void FinishUpStairMoving()
        {
            gameObject.GetComponent<Animator>().SetBool("IsStairUpping",false);
            StartCoroutine(MixedAnimeLug());
        }

        private IEnumerator MixedAnimeLug()
        {
            yield return new WaitForSeconds(0.5f);
            gameController.FinishStairUpMotion();
        }
    }
}
