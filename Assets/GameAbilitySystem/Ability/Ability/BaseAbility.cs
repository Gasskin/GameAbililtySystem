using UnityEngine;

namespace GameplayAbilitySystem.Ability
{
    public abstract class BaseAbility : ScriptableObject
    {
        [SerializeField] private string AbilityName;

        /// <summary>
        /// Tags for this ability
        /// </summary>
        [SerializeField] public AbilityTags AbilityTags;

        /// <summary>
        /// The GameplayEffect that defines the cost associated with activating the ability
        /// </summary>
        /// <param name="owner">Usually the character activating this ability</param>
        /// <returns></returns>
        [SerializeField] public GameEffect Cost;

        /// <summary>
        /// The GameplayEffect that defines the cooldown associated with this ability
        /// </summary>
        /// <param name="owner">Usually the character activating this ability</param>
        /// <returns></returns>
        [SerializeField] public GameEffect Cooldown;

        /// <summary>
        /// Creates the Ability Spec (the instantiation of the ability)
        /// </summary>
        /// <param name="owner">Usually the character casting thsi ability</param>
        /// <returns>Ability Spec</returns>
        public abstract BaseAbilitySpec CreateSpec(AbilitySystemComponent owner);
    }
}