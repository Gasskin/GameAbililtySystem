using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameAbilitySystem
{
    [CreateAssetMenu(menuName = "GameAbilitySystem/Linear Derived Attribute")]
    public class LinearDerivedGameAttribute : GameAttribute
    {
        [LabelText("目标属性")]
        [LabelWidth(50)]
        public GameAttribute Attribute;
        [LabelText("倍率")]
        [LabelWidth(50)]
        [SerializeField] private float gradient;
        [LabelText("偏移")]
        [LabelWidth(50)]
        [SerializeField] private float offset;

        public override AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue, List<AttributeValue> allAttributeValues)
        {
            // 找到目标属性
            var baseAttributeValue = allAttributeValues.Find(x => x.attribute == Attribute);

            // 基础值
            attributeValue.baseValue = (baseAttributeValue.currentValue * gradient) + offset;

            // 当前值
            attributeValue.currentValue = (attributeValue.baseValue + attributeValue.modifier.add) * (attributeValue.modifier.multiply + 1);

            if (attributeValue.modifier.overwrite != 0)
            {
                attributeValue.currentValue = attributeValue.modifier.overwrite;
            }
            return attributeValue;
        }

#if UNITY_EDITOR
        [PropertySpace]
        [Title("说明")]
        [InfoBox("这是一个线性派生属性，它的值由目标属性的值计算得出。\n" +
                "自身属性 = 目标属性 * 倍率 + 偏移")]
        [OnInspectorGUI]
        [HideIf("@this.GetType() != typeof(LinearDerivedGameAttribute)")]
        public void OnInspectorGUI() { }
#endif
    }
}