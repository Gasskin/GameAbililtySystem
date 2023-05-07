using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// 技能系统
    /// </summary>
    public class AbilitySystemComponent : MonoBehaviour
    {
    #region 属性
        [SerializeField]
        private AttributeSystemComponent attributeSystemComponent;
        public AttributeSystemComponent AttributeSystemComponent => attributeSystemComponent;

    #endregion
    }
}
