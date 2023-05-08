using Cysharp.Threading.Tasks;

namespace GameplayAbilitySystem.Ability
{
    public struct AbilityCooldownTime
    {
        public float TimeRemaining { get; set; }
        public float TotalDuration { get; set; }
    }

    public abstract class BaseAbilitySpec
    {
        /// <summary>
        /// The ability this AbilitySpec is linked to
        /// </summary>
        public BaseAbility ability;

        /// <summary>
        /// The owner of the AbilitySpec - usually the source
        /// </summary>
        protected AbilitySystemComponent owner;

        /// <summary>
        /// Ability level
        /// </summary>
        public float Level { get; set; }

        /// <summary>
        /// Is this AbilitySpec currently active?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Default constructor.  Initialises the AbilitySpec from the AbstractAbilityScriptableObject
        /// </summary>
        /// <param name="ability">Ability</param>
        /// <param name="owner">Owner - usually the character activating the ability</param>
        public BaseAbilitySpec(BaseAbility ability, AbilitySystemComponent owner)
        {
            this.ability = ability;
            this.owner = owner;
        }

        public async UniTaskVoid TryActivateAbility()
        {
            if (!CanActivateAbility())
                return;
            IsActive = true;
            await UniTask.Yield();
            await PreActivate();
            await ActivateAbility();
            EndAbility();
        }

        public bool CanActivateAbility()
        {
            return !IsActive
                   && CheckGameplayTags()
                   && CheckCost()
                   && CheckCooldown().TimeRemaining <= 0;
        }


        protected virtual async UniTask PreActivate()
        {
            await UniTask.Yield();
        }


        protected virtual async UniTask ActivateAbility()
        {
            await UniTask.Yield();
        }

        public virtual void EndAbility()
        {
            IsActive = false;
        }

        public abstract void CancelAbility();
        public abstract bool CheckGameplayTags();

        public AbilityCooldownTime CheckCooldown()
        {
            float maxDuration = 0;
            if (ability.Cooldown == null) return new AbilityCooldownTime();
            var cooldownTags = ability.Cooldown.gameplayEffectTags.grantedTags;

            float longestCooldown = 0f;

            for (var i = 0; i < owner.appliedGameplayEffects.Count; i++)
            {
                var grantedTags = owner.appliedGameplayEffects[i].spec.gameEffect.gameplayEffectTags.grantedTags;
                for (var j = 0; j < grantedTags.Length; j++)
                {
                    for (var k = 0; k < cooldownTags.Length; k++)
                    {
                        if (grantedTags[k] == cooldownTags[k])
                        {
                            if (owner.appliedGameplayEffects[k].spec.gameEffect.gameplayEffect.durationPolicy ==
                                EDurationPolicy.Infinite)
                                return new AbilityCooldownTime
                                {
                                    TimeRemaining = float.MaxValue,
                                    TotalDuration = 0
                                };

                            var durationRemaining = owner.appliedGameplayEffects[k].spec.DurationRemaining;

                            if (durationRemaining > longestCooldown)
                            {
                                longestCooldown = durationRemaining;
                                maxDuration = owner.appliedGameplayEffects[k].spec.TotalDuration;
                            }
                        }
                    }
                }
            }
            
            return new AbilityCooldownTime
            {
                TimeRemaining = longestCooldown,
                TotalDuration = maxDuration
            };
        }
        
        public bool CheckCost()
        {
            if (ability.Cost == null) return true;
            var geSpec = owner.MakeOutgoingSpec(ability.Cost, Level);
            
            if (geSpec.gameEffect.gameplayEffect.durationPolicy != EDurationPolicy.Instant) 
                return true;

            for (var i = 0; i < geSpec.gameEffect.gameplayEffect.modifiers.Length; i++)
            {
                var modifier = geSpec.gameEffect.gameplayEffect.modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modifier.modifierOperator != EAttributeModifier.Add) 
                    continue;
                var costValue = (modifier.modifierMagnitude.CalculateMagnitude(geSpec) * modifier.multiplier).GetValueOrDefault();

                owner.AttributeSystemComponent.TryGetAttributeValue(modifier.attribute, out var attributeValue);

                // The total attribute after accounting for cost should be >= 0 for the cost check to succeed
                if (attributeValue.currentValue + costValue < 0) 
                    return false;
            }
            return true;
        }

        protected bool HasAllTags(AbilitySystemComponent asc, GameTag[] tags)
        {
            if (!asc) return true;

            for (var i = 0; i < tags.Length; i++)
            {
                var abilityTag = tags[i];

                var flag = false;
                for (var j = 0; j < asc.appliedGameplayEffects.Count; j++)
                {
                    var ascGrantedTags = asc.appliedGameplayEffects[j].spec.gameEffect.gameplayEffectTags
                        .grantedTags;
                    for (var k = 0; k < ascGrantedTags.Length; k++)
                    {
                        if (ascGrantedTags[k] == abilityTag)
                        {
                            flag = true;
                        }
                    }
                }

                if (!flag) return false;
            }

            return true;
        }

        protected bool HasNoneTags(AbilitySystemComponent asc, GameTag[] tags)
        {
            if (!asc) return true;

            for (var i = 0; i < tags.Length; i++)
            {
                var abilityTag = tags[i];

                var flag = true;
                for (var j = 0; j < asc.appliedGameplayEffects.Count; j++)
                {
                    var ascGrantedTags = asc.appliedGameplayEffects[j].spec.gameEffect.gameplayEffectTags
                        .grantedTags;
                    for (var k = 0; k < ascGrantedTags.Length; k++)
                    {
                        if (ascGrantedTags[k] == abilityTag)
                        {
                            flag = false;
                        }
                    }
                }

                if (!flag) return false;
            }

            return true;
        }
    }
}