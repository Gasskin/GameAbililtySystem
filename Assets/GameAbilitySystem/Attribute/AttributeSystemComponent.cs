using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 属性系统
    /// </summary>
    public class AttributeSystemComponent : MonoBehaviour
    {
    #region 属性
        [SerializeField] [LabelText("属性事件")] 
        private List<BaseAttributeEventHandler> attributeSystemEvents;

        [SerializeField] [LabelText("属性")] 
        private List<GameAttribute> attributes;

        [SerializeField] [LabelText("属性值")]
        private List<GameAttributeValue> attributeValues;

        private bool isAttributeDirty = false;
        public readonly Dictionary<GameAttribute, int> attributeCache = new();

        private readonly List<GameAttributeValue> preAttributeValues = new();
    #endregion


    #region 生命周期
        private void Awake()
        {
            InitialiseAttributeValues();
            MarkAttributeDirty();
            GetAttributeCache();
        }

        private void LateUpdate()
        {
            UpdateAttributeCurrentValues();
        }

        private void UpdateAttributeCurrentValues()
        {
            preAttributeValues.Clear();
            for (var i = 0; i < attributeValues.Count; i++)
            {
                var attr = attributeValues[i];
                preAttributeValues.Add(attr);
                attributeValues[i] = attr.attribute.CalculateCurrentAttributeValue(attr, attributeValues);
            }

            for (var i = 0; i < attributeSystemEvents.Count; i++)
            {
                attributeSystemEvents[i].PreAttributeChange(this, preAttributeValues, ref attributeValues);
            }
        }
    #endregion

    #region 接口方法
        /// <summary>
        /// 标记属性为脏
        /// </summary>
        public void MarkAttributeDirty()
        {
            isAttributeDirty = true;
        }

        /// <summary>
        /// 获取一个属性值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="attrValue"></param>
        /// <returns></returns>
        public bool TryGetAttributeValue(GameAttribute attr, out GameAttributeValue attrValue)
        {
            var attributeCache = GetAttributeCache();
            if (attributeCache.TryGetValue(attr, out var index))
            {
                attrValue = attributeValues[index];
                return true;
            }

            attrValue = new GameAttributeValue();
            return false;
        }

        /// <summary>
        /// 设置某个属性的基础值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="baseValue"></param>
        public void SetAttributeBaseValue(GameAttribute attr, float baseValue)
        {
            var cache = GetAttributeCache();
            if (cache.TryGetValue(attr, out var index))
            {
                var attrValue = attributeValues[index];
                attrValue.baseValue = baseValue;
                attributeValues[index] = attrValue;
            }
        }

        /// <summary>
        /// 更新某个属性的修饰器
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="modifier"></param>
        public void UpdateAttributeModifier(GameAttribute attr, GameAttributeModifier modifier)
        {
            var cache = GetAttributeCache();
            if (cache.TryGetValue(attr, out var index))
            {
                var attrValue = attributeValues[index];
                attrValue.modifier = modifier;
                attributeValues[index] = attrValue;
            }
        }
        
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="attrs"></param>
        public void AddAttributes(params GameAttribute[] attrs)
        {
            var cache = GetAttributeCache();
            for (int i = 0; i < attrs.Length; i++)
            {
                if (cache.ContainsKey(attrs[i]))
                {
                    continue;
                }
                attributes.Add(attrs[i]);
                // MarkAttributeDirty();
                // cache.Add(attrs[i], attributes.Count - 1);
            }
        }

        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <param name="attrs"></param>
        public void RemoveAttributes(params GameAttribute[] attrs)
        {
            for (int i = 0; i < attrs.Length; i++)
            {
                attributes.Remove(attrs[i]);
            }
            // GetAttributeCache();
        }
        
        /// <summary>
        /// 重置所有属性的修饰器
        /// </summary>
        public void ResetAttributeModifiers()
        {
            for (var i = 0; i < attributeValues.Count; i++)
            {
                var attributeValue = attributeValues[i];
                attributeValue.modifier = default;
                attributeValues[i] = attributeValue;
            }
        }
    #endregion

    #region 工具方法
        private void InitialiseAttributeValues()
        {
            attributeValues = new List<GameAttributeValue>();
            for (var i = 0; i < attributes.Count; i++)
            {
                attributeValues.Add(new GameAttributeValue()
                    {
                        attribute = attributes[i],
                        modifier = new GameAttributeModifier()
                        {
                            add = 0f,
                            multiply = 0f,
                            overwrite = 0f
                        }
                    }
                );
            }
        }

        private Dictionary<GameAttribute, int> GetAttributeCache()
        {
            if (isAttributeDirty)
            {
                attributeCache.Clear();
                for (var i = 0; i < attributeValues.Count; i++)
                {
                    attributeCache.Add(attributeValues[i].attribute, i);
                }

                isAttributeDirty = false;
            }

            return attributeCache;
        }
    #endregion
    }
}