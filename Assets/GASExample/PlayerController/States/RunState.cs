using Animancer;
using UnityEngine;

namespace GASExample
{
    public class RunState : PlayerState
    {
        public MixerTransition2DAsset.UnShared transition;
    
        private Vector2 currentMoveInput;
        private Vector2 smoothVelocity;

        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.animancer.Play(transition);
        }

        private void Update()
        {
            transition.State.Parameter = controller.input.MoveInput;
        }
    }
}