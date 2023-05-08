using System;
using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    [Serializable]
    public struct GameplayEffectDefinitionContainer
    {
        [LabelText("时间规则")]
        public EDurationPolicy durationPolicy;
        [LabelText("时间修改器")]
        public BaseModifierMagnitude durationModifier;
        [LabelText("持续时间")]
        public float durationMultiplier;
        [LabelText("修饰器")]
        public GameplayEffectModifier[] modifiers;
        
        public ConditionalGameplayEffectContainer[] conditionalGameplayEffectContainers;
    }
}