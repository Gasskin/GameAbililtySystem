using System;
using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    [Serializable]
    public struct GameplayEffectPeriod
    {
        [LabelText("执行周期"), LabelWidth(70)]
        public float period;
        [LabelText("立刻执行"), LabelWidth(70)]
        public bool executeOnApplication;
    }
}