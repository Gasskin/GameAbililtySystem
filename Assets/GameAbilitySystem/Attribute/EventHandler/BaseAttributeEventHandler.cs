using System.Collections.Generic;
using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 属性事件处理器，会在属性改变前被调用，可以继承这个类实现自己的属性事件处理器
    /// </summary>
    public abstract class BaseAttributeEventHandler : ScriptableObject
    {
        public abstract void PreAttributeChange(AttributeSystemComponent attributeSystem, List<GameAttributeValue> prevAttributeValues, ref List<GameAttributeValue> currentAttributeValues);
    }
}