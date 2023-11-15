using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    public enum ECaptureAttributeFrom
    {
        Source,
        Target
    }

    public enum ECaptureAttributeWhen
    {
        OnCreate,
        OnApplication
    }

    /// <summary>
    /// 基于属性的数值规格，可以捕获目标属性
    /// </summary>
    [CreateAssetMenu(menuName = "GameAbilitySystem/Magnitude/Attribute Backed")]
    public class AttributeBackedMagnitude : BaseMagnitude
    {
        [Title("说明")]
        [InfoBox("Curve的采样目标是目标属性的值\n这个值会在指定的时机被捕获")]
        [LabelText("捕获目标"), LabelWidth(50)]
        [SerializeField]
        private GameAttribute capturedAttribute;

        [LabelText("捕获来源"), LabelWidth(50)]
        [SerializeField]
        private ECaptureAttributeFrom captureAttributeFrom;

        [LabelText("捕获时机"), LabelWidth(50)]
        [SerializeField]
        private ECaptureAttributeWhen captureAttributeWhen;

        [SerializeField]
        [LabelText("采样曲线"), LabelWidth(50)]
        private AnimationCurve animationCurve;

        public override void Initialise(GameEffectSpec spec)
        {
            switch (captureAttributeFrom)
            {
                case ECaptureAttributeFrom.Source:
                    spec.source.attributeSystemComponent.TryGetAttributeValue(capturedAttribute,
                        out var capturedAttributeValue);
                    spec.sourceCapturedAttributeValue = capturedAttributeValue;
                    break;
                case ECaptureAttributeFrom.Target:
                    spec.target.attributeSystemComponent.TryGetAttributeValue(capturedAttribute,
                        out capturedAttributeValue);
                    spec.targetCapturedAttributeValue = capturedAttributeValue;
                    break;
            }
        }

        public override float CalculateMagnitude(GameEffectSpec spec)
        {
            return animationCurve.Evaluate(GetCapturedAttribute(spec).currentValue);
        }

        private GameAttributeValue GetCapturedAttribute(GameEffectSpec spec)
        {
            if (captureAttributeWhen == ECaptureAttributeWhen.OnCreate)
            {
                switch (captureAttributeFrom)
                {
                    case ECaptureAttributeFrom.Source:
                        return spec.sourceCapturedAttributeValue;
                    case ECaptureAttributeFrom.Target:
                        return spec.targetCapturedAttributeValue;
                }
            }

            switch (captureAttributeFrom)
            {
                case ECaptureAttributeFrom.Source:
                    spec.source.attributeSystemComponent.TryGetAttributeValue(capturedAttribute,
                        out var sourceAttributeValue);
                    return sourceAttributeValue;
                case ECaptureAttributeFrom.Target:
                    spec.target.attributeSystemComponent.TryGetAttributeValue(capturedAttribute,
                        out var targetAttributeValue);
                    return targetAttributeValue;
            }

            return default;
        }
    }
}