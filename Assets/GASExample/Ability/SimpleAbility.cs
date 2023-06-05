using System.Collections;
using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using FlowCanvas;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace GASExample
{
    [CreateAssetMenu(menuName = "GASExample/Ability/Simple Ability")]
    public class SimpleAbility : BaseAbility
    {
        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner,
            FlowScriptController blueprintController)
        {
            var spec = new SimpleAbilitySpec(this, owner, blueprintController);
            return spec;
        }

        public class SimpleAbilitySpec : BaseAbilitySpec
        {
            private readonly FlowScriptController blueprintController;
            private readonly PlayerController controller;
            private readonly SimpleAbility simpleAbility;

            public SimpleAbilitySpec(BaseAbility ability, AbilitySystemComponent owner,
                FlowScriptController blueprintController) : base(ability, owner)
            {
                simpleAbility = ability as SimpleAbility;
                this.blueprintController = blueprintController;
                controller = owner.GetComponent<PlayerController>();
                level = owner.level;
            }

            protected override bool CheckGameTags()
            {
                return owner.HasAllTags(ability.ownerTags.requireTags)
                       && owner.HasNoTags(ability.ownerTags.ignoreTags)
                       && owner.HasAllTags(ability.sourceTags.requireTags)
                       && owner.HasNoTags(ability.sourceTags.ignoreTags)
                       && owner.HasAllTags(ability.targetTags.requireTags)
                       && owner.HasNoTags(ability.targetTags.ignoreTags);
            }

            public override void ActivateAbility()
            {
                base.ActivateAbility();
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

                blueprintController.SendEvent("OnAbilityStart", (BaseAbilitySpec)this, owner.GameObject());
            }

            public override void EndAbility()
            {
                if (!isActive)
                    return;
                base.EndAbility();
            }
        }
    }
}