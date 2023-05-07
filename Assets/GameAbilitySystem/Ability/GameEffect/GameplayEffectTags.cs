using System;
using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    [Serializable]
    public struct GameplayEffectTags
    {
        [LabelText("自身标签"), LabelWidth(70)]
        public GameTag assetTag;

        [LabelText("这个GE所附带的标签")]
        public GameTag[] grantedTags;

        [LabelText("删除这些标签的GE")]
        public GameTag[] removeGameEffectWithTag;

        [Title("开启/关闭 状态描述标签"), HideLabel]
        public GameTagRequireIgnoreContainer onGoingTagRequirements;

        [Title("执行该GE所必须的标签"), HideLabel]
        public GameTagRequireIgnoreContainer applicationTagRequirements;

        [Title("持有这些标签时，会删除这个GE"), HideLabel]
        public GameTagRequireIgnoreContainer removalTagRequirements;
    }
}