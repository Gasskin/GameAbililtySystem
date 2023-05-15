using Cysharp.Threading.Tasks;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GASExample
{
    [CreateAssetMenu(menuName = "GASExample/Ability/Initialize State Ability")]
    public class InitializeStateAbility : BaseAbility
    {
        [LabelText("初始化效果")] public GameEffect[] initialEffects;

        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            var spec = new InitializeStateAbilitySpec(this, owner);
            spec.level = owner.level;
            return spec;
        }

        public class InitializeStateAbilitySpec : BaseAbilitySpec
        {
            public InitializeStateAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
            {
            }

            protected override bool CheckGameTags()
            {
                return owner.HasAllTags( ability.ownerTags.requireTags) &&
                       !owner.HasIgnoreTags(ability.ownerTags.ignoreTags);
            }

            public override UniTask ActivateAbility()
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

                return UniTask.CompletedTask;
            }
        }
    }
}