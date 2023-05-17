using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    public enum EDurationPolicy
    {
        [LabelText("即时")] Instant,
        [LabelText("永久")] Infinite,
        [LabelText("持续时间")] HasDuration
    }

    [CreateAssetMenu(menuName = "GameAbilitySystem/GameEffect")]
    public class GameEffect : ScriptableObject
    {
    #region 标签
        [FoldoutGroup("标签")]
        [LabelText("附加标签")]
        [Tooltip("这个效果会附加的标签")]
        [LabelWidth(50)]
        public GameTag[] grantedTags;

        [FoldoutGroup("标签")]
        [LabelText("移除效果")]
        [Tooltip("移除任何带有这些标签的游戏效果")]
        [LabelWidth(50)]
        public GameTag[] removeGameEffectsWithTag;

        [FoldoutGroup("标签")]
        [LabelText("表示效果进行中的标签要求")]
        [Tooltip("这个效果在满足标签要求时会被认为是进行中的，否则会被认为是暂停的，如果是暂停的，那么这个效果带来的任何修饰都会被移除，直到恢复，期间仍然会进行倒计时")]
        public GameTagRequireIgnoreContainer onGoingTagRequirements;

        [FoldoutGroup("标签")]
        [LabelText("能否应用效果的标签要求")]
        [Tooltip("这个效果只会在满足标签要求时被应用，包含所有必须标签，且不包含任何禁止标签")]
        public GameTagRequireIgnoreContainer applicationTagRequirements;

        [FoldoutGroup("标签")]
        [LabelText("是否移除效果的标签条件")]
        [Tooltip("这个效果会在不满足标签要求时被移除，没有包含所有必须标签，或者包含了任何禁止的标签")]
        public GameTagRequireIgnoreContainer removalTagRequirements;
    #endregion
        
    #region 周期

        [FoldoutGroup("周期")] [LabelText("周期类型")] [LabelWidth(50)]
        public EDurationPolicy durationPolicy;

        [FoldoutGroup("周期")]
        [LabelText("执行周期")]
        [LabelWidth(50)]
        [Tooltip("每过多少秒执行一次")]
        [HideIf("@this.durationPolicy == EDurationPolicy.Instant")]
        public float period;

        [FoldoutGroup("周期")]
        [LabelText("立刻执行")]
        [LabelWidth(50)]
        [Tooltip("第一次执行是立刻执行，还是等待一个周期")]
        [HideIf("@this.durationPolicy == EDurationPolicy.Instant")]
        public bool executeImmediate;

        [FoldoutGroup("周期")]
        [LabelText("时间规格")]
        [LabelWidth(50)]
        [Tooltip("持续时间")]
        [HideIf("@this.durationPolicy != EDurationPolicy.HasDuration")]
        public BaseMagnitude durationMagnitude;

        [FoldoutGroup("周期")]
        [LabelText("时间缩放")]
        [LabelWidth(50)]
        [Tooltip("持续时间缩放")]
        [HideIf("@this.durationPolicy != EDurationPolicy.HasDuration")]
        public float durationMultiplier;

    #endregion

    #region 修饰器
        [LabelText("修饰器")]
        public GameEffectModifier[] modifiers;
    #endregion
    }
}