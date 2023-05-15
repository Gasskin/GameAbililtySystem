using System.Collections;
using System.Collections.Generic;
using Animancer;
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
        [LabelText("游戏动作")]
        public ClipTransition animationClip;
        [LabelText("特效")]
        public GameObject vfx;
        
        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            var spec = new SimpleAbilitySpec(this, owner);
            spec.level = owner.level;
            return spec;
        }

        public class SimpleAbilitySpec : BaseAbilitySpec
        {
            private PlayerController playerController;
            
            public SimpleAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
            {
                playerController = owner.GetComponent<PlayerController>();
            }

            protected override bool CheckGameTags()
            {
                return owner.HasAllTags(ability.ownerTags.requireTags)
                       && !owner.HasIgnoreTags( ability.ownerTags.ignoreTags)
                       && owner.HasAllTags( ability.sourceTags.requireTags)
                       && !owner.HasIgnoreTags( ability.sourceTags.ignoreTags)
                       && owner.HasAllTags( ability.targetTags.requireTags)
                       && !owner.HasIgnoreTags( ability.targetTags.ignoreTags);
            }

            public override async UniTask ActivateAbility()
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

                    playerController.PlayAnimation(simpleAbility.animationClip);
                    var vfx = Instantiate(simpleAbility.vfx);
                    vfx.transform.SetParent(owner.transform, false);
                    Destroy(vfx.gameObject,0.2f);
                }
            }
        }
    }
}