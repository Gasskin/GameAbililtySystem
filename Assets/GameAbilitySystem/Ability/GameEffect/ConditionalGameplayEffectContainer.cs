using System;
using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    [Serializable]
    public struct ConditionalGameplayEffectContainer
    {
        public GameEffect gameEffect;
        public GameTag[] requiredSourceTags;
    }
}