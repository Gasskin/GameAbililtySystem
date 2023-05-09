using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 属性的数值，有属性标记+数值组成+修改器
    /// </summary>
    [Serializable]
    public struct GameAttributeValue
    {
        [LabelText("目标属性")] [LabelWidth(50)] public GameAttribute attribute;
        [LabelText("基础值")] [LabelWidth(50)] public float baseValue;
        [LabelText("当前值")] [LabelWidth(50)] public float currentValue;

        [LabelText("修饰器")]
        [LabelWidth(50)]
        [HideIf("@this.modifier.add==0&&this.modifier.multiply==0&&this.modifier.overwrite==0")]
        public GameAttributeModifier modifier;
    }

    /// <summary>
    /// 属性的修改器，有叠加，乘加，覆写
    /// </summary>
    [Serializable]
    public struct GameAttributeModifier
    {
        [LabelText("叠加")] [LabelWidth(50)] [HideIf("@this.add==0")]
        public float add;

        [LabelText("乘加")] [LabelWidth(50)] [HideIf("@this.multiply==0")]
        public float multiply;

        [LabelText("覆写")] [LabelWidth(50)] [HideIf("@this.overwrite==0")]
        public float overwrite;

        public GameAttributeModifier Combine(GameAttributeModifier other)
        {
            other.add += add;
            other.multiply += multiply;
            other.overwrite = overwrite;
            return other;
        }
    }
}