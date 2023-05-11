using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GASExample
{
    [CreateAssetMenu(menuName = "GASExample/Ability/Simple Ability")]
    public class SimpleAbility : BaseAbility
    {
        [LabelText("游戏效果")]
        public GameEffect gameEffect;
        
        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            var spec = new SimpleAbilitySpec(this, owner);
            spec.level = owner.level;
            return spec;
        }

        public class SimpleAbilitySpec : BaseAbilitySpec
        {
            public SimpleAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
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

            public override UniTask ActivateAbility()
            {
                var simpleAbility = ability as SimpleAbility;
                if (simpleAbility)
                {
                    if (simpleAbility.coolDown)
                    {
                        var cdSpec = owner.MakeGameEffectSpec(simpleAbility.coolDown, level);
                        owner.ApplyGameEffectSpecToSelf(cdSpec);
                    }

                    if (simpleAbility.cost)
                    {
                        var costSpec = owner.MakeGameEffectSpec(simpleAbility.cost, level);
                        owner.ApplyGameEffectSpecToSelf(costSpec);
                    }
                    
                    if (simpleAbility.gameEffect)
                    {
                        var effectSpec = owner.MakeGameEffectSpec(simpleAbility.gameEffect, level);
                        owner.ApplyGameEffectSpecToSelf(effectSpec);
                    }
                }
                
                return UniTask.CompletedTask;
            }
        }
    }
}