using Cysharp.Threading.Tasks;
using FlowCanvas;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GASExample
{
    [CreateAssetMenu(menuName = "GASExample/Ability/Initialize State Ability")]
    public class InitializeStateAbility : GameAbility
    {
        [LabelText("初始化效果")] public GameEffect[] initialEffects;

        public override GameAbilitySpec CreateSpec(AbilitySystemComponent owner,FlowScriptController blueprintController)
        {
            var spec = new InitializeStateAbilitySpec(this, owner);
            spec.level = owner.level;
            return spec;
        }

        public class InitializeStateAbilitySpec : GameAbilitySpec
        {
            public InitializeStateAbilitySpec(GameAbility ability, AbilitySystemComponent owner) : base(ability, owner)
            {
            }

            protected override bool CheckGameTags()
            {
                return owner.HasAllTags( ability.ownerTags.requireTags) &&
                       owner.HasNoTags(ability.ownerTags.ignoreTags);
            }

            public override void ActivateAbility()
            {
                if (ability.coolDown)
                {
                    var cdSpec = owner.MakeGameEffectSpec(ability.coolDown, level);
                    owner.ApplyGameEffectSpecToSelf(cdSpec);
                }

                if (ability.cost)
                {
                    var costSpec = owner.MakeGameEffectSpec(ability.cost, level);
                    owner.ApplyGameEffectSpecToSelf(costSpec);
                }

                var initializeStateAbility = ability as InitializeStateAbility;
                foreach (var initialEffect in initializeStateAbility.initialEffects)
                {
                    var spec = owner.MakeGameEffectSpec(initialEffect);
                    owner.ApplyGameEffectSpecToSelf(spec);
                }
            }
        }
    }
}