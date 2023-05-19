using Cysharp.Threading.Tasks;
using GameAbilitySystem.Ability;

namespace GameAbilitySystem
{
    public struct AbilityCooldownTime
    {
        public float timeRemaining;
        public float totalDuration;
    }

    public abstract class BaseAbilitySpec
    {
        public BaseAbility ability;

        public AbilitySystemComponent owner;

        public float level;

        public bool isActive;

        public BaseAbilitySpec(BaseAbility ability, AbilitySystemComponent owner)
        {
            this.ability = ability;
            this.owner = owner;
        }

        public void TryActivateAbility()
        {
            if (!CanActivateAbility())
                return;
            ActivateAbility();
        }

        private bool CanActivateAbility()
        {
            return (!isActive || (isActive && ability.canActiveWhenActive))
                   && CheckGameTags()
                   && CheckCost()
                   && CheckCooldown().timeRemaining <= 0;
        }

        protected abstract bool CheckGameTags();

        public virtual void ActivateAbility()
        {
            isActive = true;

            var index = owner.currentAbilitySpecs.IndexOf(this);
            if (index > 0)
            {
                var current = owner.currentAbilitySpecs[index];
                if (current.ability.endSelfWhenActiveInActive)
                    current.EndAbility();
                owner.currentAbilitySpecs.Remove(this);
            }
            owner.currentAbilitySpecs.Add(this);
            
            owner.CancelAbilityWithTags(ability.cancelAbilityWithTags);
        }

        public virtual void EndAbility()
        {
            isActive = false;
            // owner.currentAbilitySpecs.Remove(this);
        }

        private bool CheckCost()
        {
            if (ability.cost == null)
                return true;
            var spec = owner.MakeGameEffectSpec(ability.cost, level);
            if (spec.gameEffect.durationPolicy != EDurationPolicy.Instant)
                return true;

            foreach (var modifier in spec.gameEffect.modifiers)
            {
                if (modifier.modifierOperator != EModifierOperator.Add)
                    continue;
                var cost = modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier;
                if (owner.attributeSystemComponent.TryGetAttributeValue(modifier.attribute, out var attributeValue))
                {
                    if (attributeValue.currentValue + cost < 0)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public AbilityCooldownTime CheckCooldown()
        {
            if (ability.coolDown == null)
                return new AbilityCooldownTime();
            var maxDuration = 0f;
            var longestCooldown = 0f;
            var coolDownTags = ability.coolDown.grantedTags;

            // 遍历ASC的所有GE，找到所有的CD GE，找到最长的CD时间
            foreach (var appliedGameEffect in owner.appliedGameEffects)
            {
                foreach (var grantedTag in appliedGameEffect.spec.gameEffect.grantedTags)
                {
                    foreach (var coolDownTag in coolDownTags)
                    {
                        if (grantedTag.IsDescendantOf(coolDownTag))
                        {
                            if (appliedGameEffect.spec.gameEffect.durationPolicy == EDurationPolicy.Infinite)
                            {
                                return new AbilityCooldownTime
                                {
                                    timeRemaining = float.MaxValue,
                                    totalDuration = 0
                                };
                            }

                            var durationRemaining = appliedGameEffect.spec.durationRemaining;
                            if (durationRemaining > longestCooldown)
                            {
                                longestCooldown = durationRemaining;
                                maxDuration = appliedGameEffect.spec.totalDuration;
                            }
                        }
                    }
                }
            }

            return new AbilityCooldownTime
            {
                timeRemaining = longestCooldown,
                totalDuration = maxDuration
            };
        }
    }
}