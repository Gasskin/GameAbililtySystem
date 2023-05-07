using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    [CreateAssetMenu(menuName = "GameAbilitySystem/Attribute EventHandler/Log Attribute Change")]
    public class LogAttributeChangeEventHandler : AbstractAttributeEventHandler
    {
        [SerializeField]
        [LabelText("目标属性")]
        [LabelWidth(50)]
        private GameAttribute primaryAttribute;
        
        public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
        {
            var attributeCacheDict = attributeSystem.attributeIndexCache;
            if (attributeCacheDict.TryGetValue(primaryAttribute, out var primaryAttributeIndex))
            {
                var prevValue = prevAttributeValues[primaryAttributeIndex].currentValue;
                var currentValue = currentAttributeValues[primaryAttributeIndex].currentValue;

                if (Math.Abs(prevValue - currentValue) > 0.0001f) 
                {
                    Debug.Log($"{attributeSystem.gameObject.name}: {currentAttributeValues[primaryAttributeIndex].attribute.name} {prevValue} >>> {currentValue}");
                }
            }
        }
    }
}
