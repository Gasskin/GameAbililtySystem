using Sirenix.OdinInspector;
using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// 一个基础修饰器，根据效果的等级，返回一个曲线上的数值
    /// </summary>
    [CreateAssetMenu(menuName = "GameplayAbilitySystem/Game Effect Modifier/Simple Float")]
    public class SimpleFloatModifierMagnitude : BaseModifierMagnitude
    {
        [SerializeField]
        [LabelText("倍率曲线"),LabelWidth(50)]
        private AnimationCurve scaleCurve;
        
        public override void Initialise(GameplayEffectSpec spec)
        {
            
        }

        public override float? CalculateMagnitude(GameplayEffectSpec spec)
        {
            return scaleCurve.Evaluate(spec.Level);
        }
    }
}