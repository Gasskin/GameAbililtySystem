﻿using System;
using System.Collections.Generic;
using FlowCanvas;
using Sirenix.OdinInspector;
using UnityEditor;
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
        public Dictionary<BaseAbility, BaseAbilitySpec> grantedAbilities = new();
        public Dictionary<BaseAbility, FlowScriptController> abilityBlueprints = new();
        public List<BaseAbilitySpec> currentAbilitySpecs = new();
        public float level;

        private Transform abilityBlueprintRoot;
#if UNITY_EDITOR
        [OnInspectorGUI]
        public void OnInspectorGUI()
        {
            var tags = "";
            foreach (var appliedGameEffect in appliedGameEffects)
            {
                foreach (var gameTag in appliedGameEffect.spec.gameEffect.grantedTags)
                {
                    tags = string.IsNullOrEmpty(tags) ? gameTag.name : $"{tags}\n{gameTag.name}";
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("游戏效果：");
            EditorGUILayout.TextArea(tags);
        }
#endif

    #region 生命周期
        private void Awake()
        {
            abilityBlueprintRoot = new GameObject("_ability_blueprint_root").transform;
            abilityBlueprintRoot.SetParent(transform, false);
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

                if (execute && HasAllTags(onGoingTags.requireTags) && HasNoTags(onGoingTags.ignoreTags))
                {
                    // ApplyInstantGameEffect(appliedGameEffect.spec);
                    // var modifierContainers = new List<ModifierContainer>();
                    foreach (var modifier in appliedGameEffect.spec.gameEffect.modifiers)
                    {
                        var magnitude = modifier.modifierMagnitude.CalculateMagnitude(appliedGameEffect.spec) *
                                        modifier.multiplier;
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

                    RemoveEffectsWithTags(appliedGameEffect.spec.gameEffect.removeGameEffectsWithTag);
                }
            }

            CleanGameEffects();
        }

        private void CleanGameEffects()
        {
            appliedGameEffects.RemoveAll(x =>
                x.spec.gameEffect.durationPolicy != EDurationPolicy.Instant && x.spec.durationRemaining <= 0f);
        }
    #endregion

    #region 接口方法
        public GameEffectSpec MakeGameEffectSpec(GameEffect gameEffect, float level = 1f)
        {
            return GameEffectSpec.CreateNew(gameEffect, this, level);
        }

        public void ApplyGameEffectSpecToSelf(GameEffectSpec spec)
        {
            if (spec == null)
                return;
            if (!HasAllTags(spec.gameEffect.applicationTagRequirements.requireTags) ||
                !HasNoTags(spec.gameEffect.applicationTagRequirements.ignoreTags))
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

        public void AddAbility(BaseAbility baseAbility)
        {
            if (!grantedAbilities.ContainsKey(baseAbility))
            {
                var blueprint = new GameObject($"{baseAbility}");
                var controller = blueprint.AddComponent<FlowScriptController>();
                controller.graph = baseAbility.blueprint;
                blueprint.transform.SetParent(abilityBlueprintRoot, false);
                controller.StartBehaviour();
                
                grantedAbilities.Add(baseAbility, baseAbility.CreateSpec(this, controller));
            }
        }

        public void ActiveAbility(BaseAbility baseAbility)
        {
            if (grantedAbilities.TryGetValue(baseAbility, out var spec))
            {
                spec.TryActivateAbility();
            }
        }

        public void CancelAbilityWithTags(GameTag[] gameTags)
        {
            if (gameTags == null || gameTags.Length <= 0)
                return;

            foreach (var abilitySpec in currentAbilitySpecs)
            {
                foreach (var gameTag in gameTags)
                {
                    if (abilitySpec.ability.assetTag && abilitySpec.ability.assetTag.IsDescendantOf(gameTag))
                    {
                        abilitySpec.EndAbility();
                        break;
                    }
                }
            }

            currentAbilitySpecs.RemoveAll(x => !x.isActive);
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

        public bool HasNoTags(GameTag[] tags)
        {
            if (tags == null || tags.Length == 0)
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
    #endregion

    #region 工具方法
        private void ApplyInstantGameEffect(GameEffectSpec spec)
        {
            foreach (var modifier in spec.gameEffect.modifiers)
            {
                if (attributeSystemComponent.TryGetAttributeValue(modifier.attribute, out var attributeValue))
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

            RemoveEffectsWithTags(spec.gameEffect.removeGameEffectsWithTag);
            CleanGameEffects();
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

        private void RemoveEffectsWithTags(GameTag[] gameTags)
        {
            foreach (var gameTag in gameTags)
            {
                foreach (var appliedGameEffect in appliedGameEffects)
                {
                    foreach (var grantedTag in appliedGameEffect.spec.gameEffect.grantedTags)
                    {
                        if (grantedTag.IsDescendantOf(gameTag))
                        {
                            appliedGameEffect.spec.durationRemaining = 0f;
                            break;
                        }
                    }
                }
            }
        }
    #endregion
    }
}