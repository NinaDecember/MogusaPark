namespace Sho_Project
{

    using UnityEngine;
    using System.Linq;

    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private EffectData effectDatas;

        public void StartEffect(string effectName)
        {
            Effect effect = effectDatas.effectDatas.FirstOrDefault(x => x.name == effectName);
            GameObject effectObj = effect.effectObj;
            Vector3 pos = effect.pos;
            Instantiate(effectObj, pos, Quaternion.identity);
        }

        public void StartEffect_Pos(string effectName, Vector3 pos)
        {
            Effect effect = effectDatas.effectDatas.FirstOrDefault(x => x.name == effectName);
            GameObject effectObj = effect.effectObj;
            Instantiate(effectObj, pos, Quaternion.identity);
        }
    }

}