using GameAbilitySystem;
using GameAbilitySystem.Ability;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCD : MonoBehaviour
{
    public Image bg;
    public BaseAbility ability;
    public AbilitySystemComponent asc;

    void Update()
    {
        if (asc.grantedAbilities.TryGetValue(ability,out var spec))
        {
            var cooldown = spec.CheckCooldown();
            bg.fillAmount = cooldown.timeRemaining / cooldown.totalDuration;
        }
    }
}
