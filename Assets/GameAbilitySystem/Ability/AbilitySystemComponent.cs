using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// 技能系统
    /// </summary>
    public class AbilitySystemComponent : MonoBehaviour
    {
    #region 属性
        public float level;
        
        [SerializeField]
        private AttributeSystemComponent attributeSystemComponent;
        public AttributeSystemComponent AttributeSystemComponent => attributeSystemComponent;

        public List<GameplayEffectContainer> appliedGameplayEffects = new List<GameplayEffectContainer>();
    #endregion
        
        
        public GameplayEffectSpec MakeOutgoingSpec(GameEffect gameEffect, float? level = 1f)
        {
            level ??= this.level;
            return GameplayEffectSpec.CreateNew(gameEffect, this, level.GetValueOrDefault(1));
        }
    }
}
