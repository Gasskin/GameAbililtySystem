using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// 修改器规范
    /// </summary>
    public abstract class BaseModifierMagnitude : ScriptableObject
    {
        public abstract void Initialise(GameplayEffectSpec spec);
        public abstract float? CalculateMagnitude(GameplayEffectSpec spec);
    }
}