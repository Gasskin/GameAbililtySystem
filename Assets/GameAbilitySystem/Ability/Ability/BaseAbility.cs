using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    public abstract class BaseAbility : ScriptableObject
    {
        [LabelText("技能名称")]
        [LabelWidth(50)]
        private string abilityName;

        [FoldoutGroup("标签")]
        [LabelText("自身标签")]
        [LabelWidth(50)]
        public GameTag assetTag;
        
        [FoldoutGroup("标签")]
        [LabelText("取消带有这些标签的能力")]
        [Tooltip("举例，玩家A有两个技能，一个火魔法，一个冰魔法，当我们释放火魔法时，想要打断自身的所有冰魔法，那么这里就需要加上冰魔法的标签")]
        public GameTag[] cancelAbilityWithTags;

        [FoldoutGroup("标签")]
        [LabelText("持有这些标签时该能力无法激活")]
        public GameTag[] blockAbilityWithTags;

        [FoldoutGroup("标签")]
        [LabelText("激活能力时的附带标签")]
        public GameTag[] activationOwnedTags;

        [FoldoutGroup("标签")]
        [LabelText("持有者的标签条件")]
        [Tooltip("当技能持有者不满足标签条件时，无法释放技能")]
        public GameTagRequireIgnoreContainer ownerTags;
        
        [FoldoutGroup("标签")]
        [LabelText("技能源的标签条件")]
        [Tooltip("当技能释放者不满足标签条件时，无法释放技能，只有通过事件触发技能时，会设置这个标签")]
        public GameTagRequireIgnoreContainer sourceTags;

        [FoldoutGroup("标签")]
        [LabelText("技能目标的标签条件")]
        [Tooltip("当技能目标不满足标签条件时，无法释放技能，只有通过事件触发技能时，会设置这个标签")]
        public GameTagRequireIgnoreContainer targetTags;


        [LabelText("冷却时间")]
        public GameEffect coolDown;

        [LabelText("技能消耗")]
        public GameEffect cost;
        
        public abstract BaseAbilitySpec CreateSpec(AbilitySystemComponent owner);
    }
}