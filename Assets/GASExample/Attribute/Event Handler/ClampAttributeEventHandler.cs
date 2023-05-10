using System.Collections.Generic;
using GameAbilitySystem;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GASExample/Attribute EventHandler/Clamp Attribute")]
public class ClampAttributeEventHandler : BaseAttributeEventHandler
{
    [SerializeField]
    [LabelText("基础值")]
    [LabelWidth(50)]
    private GameAttribute primaryAttribute;
    [SerializeField]
    [LabelText("最大值")]
    [LabelWidth(50)]
    private GameAttribute maxAttribute;
    
    public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<GameAttributeValue> prevAttributeValues, ref List<GameAttributeValue> currentAttributeValues)
    {
        var attributeCacheDict = attributeSystem.attributeCache;
        ClampAttributeToMax(primaryAttribute, maxAttribute, currentAttributeValues, attributeCacheDict);
    }

    private void ClampAttributeToMax(GameAttribute pAttr, GameAttribute mAttr, List<GameAttributeValue> attributeValues, Dictionary<GameAttribute, int> attributeCacheDict)
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
