using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 简单的数值规格，可以通过曲线编辑
    /// </summary>
    [CreateAssetMenu(menuName = "GameAbilitySystem/Magnitude/Simple Float")]
    public class SimpleFloatMagnitude : BaseMagnitude
    {
        [Title("说明")]
        [InfoBox("最基础的规格器\nCurve的采样目标是所属GES的Level")]
        [SerializeField]
        [LabelText("采样曲线"), LabelWidth(50)]
        private AnimationCurve animationCurve;
        
        public override void Initialise(GameEffectSpec spec)
        {
        }

        public override float CalculateMagnitude(GameEffectSpec spec)
        {
            return animationCurve.Evaluate(spec.Level);
        }
    }
}