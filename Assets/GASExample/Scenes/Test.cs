using System.Collections;
using System.Collections.Generic;
using Animancer;
using FlowCanvas;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using NodeCanvas.Framework;
using UnityEngine;

public class Test : MonoBehaviour
{
    public FlowScript flowScript;
    public AbilitySystemComponent asc;

    public ClipTransitionAsset.UnShared unShared;
    
    void Start()
    {
        BlueprintManager.Instance.StartBlueprint(flowScript);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            BlueprintManager.Instance.StartBlueprint(flowScript);
        }
    }
}
