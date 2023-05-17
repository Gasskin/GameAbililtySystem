using Animancer;
using UnityEngine;

namespace GASExample
{
    public class AttackState : PlayerState
    {
        public ClipTransition transition; 
            
        private void OnEnable()
        {
            controller.animancer.Play(transition);
        }
    }
}