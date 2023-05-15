using System.Collections;
using System.Collections.Generic;
using GameAbilitySystem.Ability;
using UnityEngine;

public class CastPointComponent : MonoBehaviour
{
    public AbilitySystemComponent target;
    
    public bool GetTarget(out AbilitySystemComponent target)
    {
        target = this.target;
        return true;
    }
}
