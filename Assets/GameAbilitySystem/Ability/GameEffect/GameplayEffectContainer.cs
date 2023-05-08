namespace GameplayAbilitySystem
{
    public class GameplayEffectContainer
    {
        public GameplayEffectSpec spec;
        public ModifierContainer[] modifiers;

        public class ModifierContainer
        {
            public GameAttribute Attribute;
            public GameAttributeModifier Modifier;
        }
    }
}