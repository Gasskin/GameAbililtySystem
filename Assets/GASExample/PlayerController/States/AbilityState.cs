using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace GASExample
{
    public class AbilityState : PlayerState
    {
        public ClipTransition transition;

        public void Prepare(ClipTransition transition)
        {
            this.transition = transition;
            transition.State.Events.OnEnd = controller.stateMachine.ForceSetDefaultState;
        }
        
        public void OnEnable()
        {
            controller.animancer.Play(transition);
        }
    }
}
