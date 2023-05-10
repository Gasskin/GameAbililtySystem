using GameAbilitySystem;
using GameAbilitySystem.Ability;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BaseAbility ability;

    public AbilitySystemComponent asc;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var spec = ability.CreateSpec(asc);
            spec.TryActivateAbility();
        }
    }
}
