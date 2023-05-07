using System;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// 存储游戏效果的具体数据
    /// </summary>
    [Serializable]
    public class GameplayEffectSpec
    {
    #region 属性
        // 等级
        public float Level { get; private set; }
        // 释放者
        public AbilitySystemComponent Source { get; private set; }
        // 目标
        public AbilitySystemComponent Target { get; private set; }
        // 捕获的源属性
        public GameAttributeValue? sourceCapturedAttribute = null;
    #endregion
    }
}