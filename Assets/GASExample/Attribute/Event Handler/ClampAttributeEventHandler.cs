using System.Collections.Generic;
using GameAbilitySystem;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GameAbilitySystem/Attribute EventHandler/Clamp Attribute")]
public class ClampAttributeEventHandler : AbstractAttributeEventHandler
{
    [SerializeField]
    [LabelText("基础值")]
    [LabelWidth(50)]
    private GameAttribute primaryAttribute;
    [SerializeField]
    [LabelText("最大值")]
    [LabelWidth(50)]
    private GameAttribute maxAttribute;
    
    public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
    {
        var attributeCacheDict = attributeSystem.attributeIndexCache;
        ClampAttributeToMax(primaryAttribute, maxAttribute, currentAttributeValues, attributeCacheDict);
    }

    private void ClampAttributeToMax(GameAttribute pAttr, GameAttribute mAttr, List<AttributeValue> attributeValues, Dictionary<GameAttribute, int> attributeCacheDict)
    {
        if (attributeCacheDict.TryGetValue(pAttr, out var primaryAttributeIndex) && attributeCacheDict.TryGetValue(mAttr, out var maxAttributeIndex))
        {
            var pAttrValue = attributeValues[primaryAttributeIndex];
            var mAttrValue = attributeValues[maxAttributeIndex];

            if (pAttrValue.currentValue > mAttrValue.currentValue) 
                pAttrValue.currentValue = mAttrValue.currentValue;
            if (pAttrValue.baseValue > mAttrValue.baseValue) 
                pAttrValue.baseValue = mAttrValue.baseValue;
            
            attributeValues[primaryAttributeIndex] = pAttrValue;
        }
    }

#if UNITY_EDITOR
    [PropertySpace]
    [Title("说明")]
    [InfoBox("将一个属性的当前值限制在他的基础值和最大值范围内。")]
    [OnInspectorGUI]
    private void OnInspectorGUI(){}
#endif
}
