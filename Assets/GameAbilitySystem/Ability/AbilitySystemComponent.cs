using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameAbilitySystem.Ability
{
    public class ModifierContainer
    {
        public GameAttribute attribute;
        public GameAttributeModifier modifier;
    }

    public class GameEffectContainer
    {
        public GameEffectSpec spec;
        public List<ModifierContainer> modifiers;
    }

    public class AbilitySystemComponent : MonoBehaviour
    {
        public AttributeSystemComponent attributeSystemComponent;

        public readonly List<GameEffectContainer> appliedGameEffects = new();
        public List<BaseAbilitySpec> grantedAbilities = new();

        public float level;

        public GameEffectSpec MakeGameEffectSpec(GameEffect gameEffect, float level = 1f)
        {
            return GameEffectSpec.CreateNew(gameEffect, this, level);
        }


        private void Update()
        {
            attributeSystemComponent.ResetAttributeModifiers();
            UpdateAttributeSystem();
            TickGameEffects();
            CleanGameEffects();
        }

        private void UpdateAttributeSystem()
        {
            foreach (var appliedGameEffect in appliedGameEffects)
            {
                foreach (var modifier in appliedGameEffect.modifiers)
                {
                    attributeSystemComponent.UpdateAttributeModifier(modifier.attribute, modifier.modifier);
                }
            }
        }

        private void TickGameEffects()
        {
            foreach (var appliedGameEffect in appliedGameEffects)
            {
                if (appliedGameEffect.spec.gameEffect.durationPolicy == EDurationPolicy.Instant)
                    continue;
                appliedGameEffect.spec.UpdateRemainingDuration(Time.deltaTime);
                appliedGameEffect.spec.TickPeriod(Time.deltaTime, out var execute);

                var onGoingTags = appliedGameEffect.spec.gameEffect.onGoingTagRequirements;
                
                if (execute)
                {
                    // ApplyInstantGameEffect(appliedGameEffect.spec);
                    // var modifierContainers = new List<ModifierContainer>();
                    foreach (var modifier in appliedGameEffect.spec.gameEffect.modifiers)
                    {
                        var magnitude = modifier.modifierMagnitude.CalculateMagnitude(appliedGameEffect.spec) * modifier.multiplier;
                        var attributeModifier = new GameAttributeModifier();
                        switch (modifier.modifierOperator)
                        {
                            case EModifierOperator.Add:
                                attributeModifier.add = magnitude;
                                break;
                            case EModifierOperator.Multiply:
                                attributeModifier.multiply = magnitude;
                                break;
                            case EModifierOperator.Override:
                                attributeModifier.overwrite = magnitude;
                                break;
                        }
                        // modifierContainers.Add(new ModifierContainer
                        // {
                        //     attribute = modifier.attribute,
                        //     modifier = attributeModifier
                        // });
                        appliedGameEffect.modifiers.Add(new ModifierContainer()
                        {
                            attribute = modifier.attribute,
                            modifier = attributeModifier
                        });
                    }
                }
            }
        }

        private void CleanGameEffects()
        {
            appliedGameEffects.RemoveAll(x =>
                x.spec.gameEffect.durationPolicy == EDurationPolicy.HasDuration && x.spec.durationRemaining <= 0f);
        }

        public void ApplyGameEffectSpecToSelf(GameEffectSpec spec)
        {
            if (spec == null)
                return;
            if (!HasAllTags(spec.gameEffect.applicationTagRequirements.requireTags) 
                || HasIgnoreTags(spec.gameEffect.applicationTagRequirements.ignoreTags))
                return;
            switch (spec.gameEffect.durationPolicy)
            {
                case EDurationPolicy.Infinite:
                case EDurationPolicy.HasDuration:
                    ApplyDurationGameEffect(spec);
                    break;
                case EDurationPolicy.Instant:
                    ApplyInstantGameEffect(spec);
                    break;
            }
        }

        private void ApplyInstantGameEffect(GameEffectSpec spec)
        {
            foreach (var modifier in spec.gameEffect.modifiers)
            {
                if (attributeSystemComponent.TryGetAttributeValue(modifier.attribute,out var attributeValue))
                {
                    var value = modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier;
                    switch (modifier.modifierOperator)
                    {
                        case EModifierOperator.Add:
                            attributeValue.baseValue += value;
                            break;
                        case EModifierOperator.Multiply:
                            attributeValue.baseValue *= value;
                            break;
                        case EModifierOperator.Override:
                            attributeValue.baseValue = value;
                            break;
                    }

                    attributeSystemComponent.SetAttributeBaseValue(modifier.attribute, attributeValue.baseValue);
                }
            }
        }

        private void ApplyDurationGameEffect(GameEffectSpec spec)
        {
            // var modifierContainers = new List<ModifierContainer>();
            // foreach (var modifier in spec.gameEffect.modifiers)
            // {
            //     var magnitude = modifier.modifierMagnitude.CalculateMagnitude(spec) * modifier.multiplier;
            //     var attributeModifier = new GameAttributeModifier();
            //     switch (modifier.modifierOperator)
            //     {
            //         case EModifierOperator.Add:
            //             attributeModifier.add = magnitude;
            //             break;
            //         case EModifierOperator.Multiply:
            //             attributeModifier.multiply = magnitude;
            //             break;
            //         case EModifierOperator.Override:
            //             attributeModifier.overwrite = magnitude;
            //             break;
            //     }
            //     modifierContainers.Add(new ModifierContainer
            //     {
            //         attribute = modifier.attribute,
            //         modifier = attributeModifier
            //     });
            // }
            appliedGameEffects.Add(new GameEffectContainer
            {
                spec = spec,
                // modifiers = modifierContainers.ToArray()
                modifiers = new List<ModifierContainer>()
            });
        }


        public bool HasAllTags(GameTag[] tags)
        {
            if (tags == null)
                return true;
            
            foreach (var tag in tags)
            {
                var flag = false;
                foreach (var appliedGameEffect in appliedGameEffects)
                {
                    foreach (var grantedTag in appliedGameEffect.spec.gameEffect.grantedTags)
                    {
                        if (grantedTag.IsDescendantOf(tag))
                            flag = true;
                    }
                }

                if (!flag)
                    return false;
            }

            return true;
        }
        
        public bool HasIgnoreTags(GameTag[] tags)
        {
            if (tags == null)
                return true;

            foreach (var tag in tags)
            {
                var flag = true;
                foreach (var appliedGameEffect in appliedGameEffects)
                {
                    foreach (var grantedTag in appliedGameEffect.spec.gameEffect.grantedTags)
                    {
                        if (grantedTag.IsDescendantOf(tag))
                            flag = false;
                    }
                }

                if (!flag)
                    return false;
            }

            return true;
        }
    }
}