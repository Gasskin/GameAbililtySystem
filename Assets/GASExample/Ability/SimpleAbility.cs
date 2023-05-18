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
        [LabelText("前摇效果")]
        public GameEffect startEffect;
        [LabelText("命中效果")]
        public GameEffect hitPointEffect;
        [LabelText("后摇效果")]
        public GameEffect backSwingEffect;
        [LabelText("游戏动作")]
        [EventNames("Start", "HitPoint", "BackSwing")]
        public ClipTransition transition;
        
        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            var spec = new SimpleAbilitySpec(this, owner)
            {
                transition = transition,
            };
            spec.level = owner.level;
            return spec;
        }

        public class SimpleAbilitySpec : BaseAbilitySpec
        {
            public ClipTransition transition;
            private PlayerController controller;
            private SimpleAbility simpleAbility;
            
            public SimpleAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
            {
                simpleAbility = ability as SimpleAbility;
                controller = owner.GetComponent<PlayerController>();
            }

            protected override bool CheckGameTags()
            {
                if (owner.HasAllTags(simpleAbility.startEffect.grantedTags))
                    return false;
                return owner.HasAllTags(ability.ownerTags.requireTags)
                       && owner.HasNoTags( ability.ownerTags.ignoreTags)
                       && owner.HasAllTags( ability.sourceTags.requireTags)
                       && owner.HasNoTags( ability.sourceTags.ignoreTags)
                       && owner.HasAllTags( ability.targetTags.requireTags)
                       && owner.HasNoTags( ability.targetTags.ignoreTags);
            }

            public override UniTask PreActivate()
            {
                controller.PrepareAbility(this);

                return base.PreActivate();
            }

            public override UniTask ActivateAbility()
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
                
                controller.EnterAbility();

                return base.ActivateAbility();
            }

            public void OnStart()
            {
                var spec = owner.MakeGameEffectSpec(simpleAbility.startEffect, level);
                owner.ApplyGameEffectSpecToSelf(spec);
            }
        
            public void OnHitPoint()
            {
            }
        
            public void OnBackSwing()
            {
                var spec = owner.MakeGameEffectSpec(simpleAbility.backSwingEffect, level);
                owner.ApplyGameEffectSpecToSelf(spec);
            }

            public void OnEnd()
            {
                
            }
        }
    }
}