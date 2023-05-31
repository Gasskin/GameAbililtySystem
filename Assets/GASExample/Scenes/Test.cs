using System.Collections;
using System.Collections.Generic;
using FlowCanvas;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using NodeCanvas.Framework;
using UnityEngine;

public class Test : MonoBehaviour
{
    public FlowScript flowScript;
    public AbilitySystemComponent asc;
    
    void Start()
    {
        BlueprintManager.Instance.StartBlueprint(flowScript);
    }

    void Update()
    {
        
    }
}
