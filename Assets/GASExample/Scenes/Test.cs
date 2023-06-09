using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Editor;
using FlowCanvas;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3 pos = Vector3.zero;
    public float radius = 10;


    void Start()
    {
        var a = LayerMask.NameToLayer("Default");
        Debug.LogError(a);
        // var target = RaycastUtil.SphereCast(new Vector3(0, 0, 0), 1, 1,LayerMask.NameToLayer("Default"));
        // for (int i = 0; i < target.Count; i++)
        // {
        // Debug.LogError(target[i].gameObject.name);
        // }
    }

    private void Update()
    {
        var target = RaycastUtil.SphereCast(pos, radius, 1, "Role");
        for (int i = 0; i < target.Count; i++)
        {
            Debug.LogError(target[i].gameObject.name);
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pos, radius);
    }
}