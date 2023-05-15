using Cysharp.Threading.Tasks;
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
        [LabelText("子弹")]
        public Projectile projectile;
        
        public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
        {
            var spec = new ProjectileAbilitySpec(this, owner);
            spec.level = owner.level;
            spec.projectile = projectile;
            spec.castPointComponent = owner.GetComponent<CastPointComponent>();
            return spec;
        }
        
        public class ProjectileAbilitySpec : BaseAbilitySpec
        {
            public Projectile projectile;
            public CastPointComponent castPointComponent;

            public ProjectileAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
            {
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
                if (ability is ProjectileAbility projectileAbility)
                {
                    if (castPointComponent.GetTarget(out var target))
                    {
                        var transform = castPointComponent.transform;
                        var go = Instantiate(projectile.gameObject, transform.position,
                            transform.rotation);
                        var projectMono = go.GetComponent<Projectile>();
                        projectMono.source = owner;
                        projectMono.target = target;
                        
                        if (projectileAbility.coolDown)
                        {
                            var cdSpec = owner.MakeGameEffectSpec(projectileAbility.coolDown, level);
                            owner.ApplyGameEffectSpecToSelf(cdSpec);
                        }

                        if (projectileAbility.cost)
                        {
                            var costSpec = owner.MakeGameEffectSpec(projectileAbility.cost, level);
                            owner.ApplyGameEffectSpecToSelf(costSpec);
                        }

                        await projectMono.TravelToTarget();
                        var effect = owner.MakeGameEffectSpec(projectileAbility.gameEffect);
                        owner.ApplyGameEffectSpecToSelf(effect);
                        
                        DestroyImmediate(go.gameObject);
                    }
                }
            }
        }
    }
}









