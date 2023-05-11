using GameAbilitySystem;
using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GASExample
{
    [CreateAssetMenu(menuName = "GASExample/Ability/Projectile Ability")]
    public class ProjectileAbility : BaseAbility
    {
        [LabelText("游戏效果")]
        public GameEffect gameEffect;
        
        
        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            var spec = new ProjectileAbilitySpec(this, owner);

            return spec;
        }
        
        public class ProjectileAbilitySpec : BaseAbilitySpec
        {
            public ProjectileAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
            {
            }

            protected override bool CheckGameTags()
            {
                return HasAllTags(owner, ability.ownerTags.requireTags)
                       && HasNoneTags(owner, ability.ownerTags.ignoreTags)
                       && HasAllTags(owner, ability.sourceTags.requireTags)
                       && HasNoneTags(owner, ability.sourceTags.ignoreTags)
                       && HasAllTags(owner, ability.targetTags.requireTags)
                       && HasNoneTags(owner, ability.targetTags.ignoreTags);
            }
        }
    }
}