using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    public class AttributeSystemComponent : MonoBehaviour
    {
    #region 属性
        [SerializeField]
        [LabelText("属性事件")]
        private List<AbstractAttributeEventHandler> attributeSystemEvents;

        [SerializeField]
        [LabelText("属性")]
        private List<GameAttribute> attributes;

        [SerializeField]
        [LabelText("属性值")]
        // [ReadOnly]
        private List<AttributeValue> attributeValues;
        
        private bool isAttributeDirty = false;
        public readonly Dictionary<GameAttribute, int> attributeIndexCache  = new ();
        
        private List<AttributeValue> preAttributeValues = new List<AttributeValue>();
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
    #endregion

    #region 接口方法
        public void MarkAttributeDirty()
        {
            isAttributeDirty = true;
        }
        
        public void UpdateAttributeCurrentValues()
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

    #region 工具方法
        private void InitialiseAttributeValues()
        {
            attributeValues = new List<AttributeValue>();
            for (var i = 0; i < attributes.Count; i++)
            {
                this.attributeValues.Add(new AttributeValue()
                    {
                        attribute = this.attributes[i],
                        modifier = new AttributeModifier()
                        {
                            add = 1f,
                            multiply = 1f,
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
                attributeIndexCache.Clear();
                for (var i = 0; i < attributeValues.Count; i++)
                {
                    attributeIndexCache.Add(attributeValues[i].attribute, i);
                }
                this.isAttributeDirty = false;
            }
            return attributeIndexCache;
        }
    #endregion
    }
}