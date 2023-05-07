using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "GameplayAbilitySystem/Attribute EventHandler/Log Attribute Change")]
    public class LogAttributeChangeEventHandler : BaseAttributeEventHandler
    {
        [SerializeField]
        [LabelText("目标属性")]
        [LabelWidth(50)]
        private GameAttribute primaryAttribute;
        
        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<GameAttributeValue> prevAttributeValues, ref List<GameAttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.attributeCache;
            if (attributeCacheDict.TryGetValue(primaryAttribute, out var primaryAttributeIndex))
            {
                var prevValue = prevAttributeValues[primaryAttributeIndex].currentValue;
                var currentValue = currentAttributeValues[primaryAttributeIndex].currentValue;

                if (Math.Abs(prevValue - currentValue) > 0.0001f) 
                {
                    Debug.Log($"【{attributeSystem.gameObject.name}】 【{currentAttributeValues[primaryAttributeIndex].attribute.name}】 {prevValue} >>> {currentValue}");
                }
            }
        }
    }
}
