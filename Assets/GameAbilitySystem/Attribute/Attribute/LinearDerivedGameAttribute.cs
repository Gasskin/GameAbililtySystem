using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 衍生属性，比如角色的蓝量是基于他的智力的，那么蓝量就是一个衍生属性，需要有他的源属性，和计算规则
    /// </summary>
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

        public override GameAttributeValue CalculateCurrentAttributeValue(GameAttributeValue gameAttributeValue, List<GameAttributeValue> allAttributeValues)
        {
            // 找到目标属性
            var baseAttributeValue = allAttributeValues.Find(x => x.attribute == Attribute);

            // 基础值
            gameAttributeValue.baseValue = (baseAttributeValue.currentValue * gradient) + offset;

            // 当前值
            gameAttributeValue.currentValue = (gameAttributeValue.baseValue + gameAttributeValue.modifier.add) * (gameAttributeValue.modifier.multiply + 1);

            if (gameAttributeValue.modifier.overwrite != 0)
            {
                gameAttributeValue.currentValue = gameAttributeValue.modifier.overwrite;
            }
            return gameAttributeValue;
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