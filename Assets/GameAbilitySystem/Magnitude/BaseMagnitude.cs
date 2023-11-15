using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 规格，根据需求返回一个特定的数值
    /// </summary>
    public abstract class BaseMagnitude : ScriptableObject
    {
        public abstract void Initialise(GameEffectSpec spec);
        public abstract float CalculateMagnitude(GameEffectSpec spec);
    }
}