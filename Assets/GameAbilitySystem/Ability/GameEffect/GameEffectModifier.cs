using System;
using Sirenix.OdinInspector;

namespace GameAbilitySystem
{
    public enum EModifierOperator
    {
        [LabelText("加")]
        Add, 
        [LabelText("乘")]
        Multiply,
        [LabelText("覆写")]
        Override
    }
    
    
    [Serializable]
    public struct GameEffectModifier
    {
        [LabelText("目标属性")]
        [LabelWidth(50)]
        public GameAttribute attribute;
        [LabelText("操作类型")]
        [LabelWidth(50)]
        public EModifierOperator modifierOperator;
        [LabelText("数值规格")]
        [LabelWidth(50)]
        public BaseMagnitude modifierMagnitude;
        [LabelText("数值缩放")]
        [LabelWidth(50)]
        public float multiplier;
    }
}