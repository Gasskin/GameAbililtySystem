using Sirenix.OdinInspector;
using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// 支持捕获目标的修改器
    /// </summary>
    [CreateAssetMenu(menuName = "GameplayAbilitySystem/Game Effect Modifier/Attribute Backed")]
    public class AttributeBackedModifierMagnitude : BaseModifierMagnitude
    {
        [SerializeField]
        [LabelText("倍率曲线"),LabelWidth(50)]
        private AnimationCurve scaleFunction;

        [SerializeField]
        [LabelText("目标属性"),LabelWidth(50)]
        private GameAttribute captureAttribute;

        [SerializeField]
        [LabelText("捕获目标"),LabelWidth(50)]
        private ECaptureAttributeFrom captureAttributeFrom;

        [SerializeField]
        [LabelText("捕获时机"),LabelWidth(50)]
        private ECaptureAttributeWhen captureAttributeWhen;
        
        
        public override void Initialise(GameplayEffectSpec spec)
        {
            spec.Source.AttributeSystemComponent.TryGetAttributeValue(captureAttribute, out var sourceAttributeValue);
            spec.sourceCapturedAttribute = sourceAttributeValue;
        }
        
        public override float? CalculateMagnitude(GameplayEffectSpec spec)
        {
            return scaleFunction.Evaluate(GetCapturedAttribute(spec).GetValueOrDefault().currentValue);
        }

        private GameAttributeValue? GetCapturedAttribute(GameplayEffectSpec spec)
        {
            if (captureAttributeWhen == ECaptureAttributeWhen.OnApplication && captureAttributeFrom == ECaptureAttributeFrom.Source)
            {
                return spec.sourceCapturedAttribute;
            }

            switch (captureAttributeFrom)
            {
                case ECaptureAttributeFrom.Source:
                    spec.Source.AttributeSystemComponent.TryGetAttributeValue(captureAttribute, out var sourceAttributeValue);
                    return sourceAttributeValue;
                case ECaptureAttributeFrom.Target:
                    spec.Target.AttributeSystemComponent.TryGetAttributeValue(captureAttribute, out var targetAttributeValue);
                    return targetAttributeValue;
                default:
                    return null;
            }
        }
    }
}