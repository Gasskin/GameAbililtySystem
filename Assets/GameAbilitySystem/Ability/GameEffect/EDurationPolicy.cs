using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    public enum EDurationPolicy
    {
        [LabelText("立刻")]
        Instant,
        [LabelText("永久")]
        Infinite,
        [LabelText("持续时间")]
        HasDuration
    }
}