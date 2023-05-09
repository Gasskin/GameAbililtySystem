using GameAbilitySystem.Ability;

namespace GameAbilitySystem
{
    public class GameEffectSpec
    {
        public float Level { get; set; }
     
        public GameAttributeValue SourceCapturedAttributeValue { get; set; }
        public GameAttributeValue TargetCapturedAttributeValue { get; set; }
        public AbilitySystemComponent Source { get; set; }
        public AbilitySystemComponent Target { get; set; }
    }
}