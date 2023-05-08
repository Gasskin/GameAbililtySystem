using System;
using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    [Serializable]
    public struct GameplayEffectModifier
    {
        [LabelText("目标属性")]
        public GameAttribute attribute;
        [LabelText("属性操作")]
        public EAttributeModifier modifierOperator;
        [LabelText("规范")]
        public BaseModifierMagnitude modifierMagnitude;
        [LabelText("乘数")]
        public float multiplier;
    }
}