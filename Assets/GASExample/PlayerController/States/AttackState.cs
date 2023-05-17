using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Examples.Events;
using UnityEngine;

namespace GASExample
{
    public class AttackState : PlayerState
    {
        public ClipTransition attack1;
        public ClipTransition attack2;
        public ClipTransition attack3;

        private void Awake()
        {
            // attack1.Events.OnEnd = controller.stateMachine.ForceSetDefaultState;
            // attack2.Events.OnEnd = controller.stateMachine.ForceSetDefaultState;
            // attack3.Events.OnEnd = controller.stateMachine.ForceSetDefaultState;
        }
        
        public override void UpdateState()
        {
        }

        private void OnEnable()
        {
            controller.animancer.Play(attack1);
        }
    }
}