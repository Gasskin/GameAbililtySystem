using Sirenix.OdinInspector;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "GameplayAbilitySystem/Gameplay Effect")]
    public class GameEffect : ScriptableObject
    {
        [LabelText("标签"),LabelWidth(50)]
        public GameplayEffectTags gameplayEffectTags;
        
        [LabelText("周期"),LabelWidth(50)]
        public GameplayEffectPeriod period;
    }
}