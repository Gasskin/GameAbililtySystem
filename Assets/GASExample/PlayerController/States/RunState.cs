using Animancer;
using UnityEngine;

namespace GASExample
{
    public class RunState : PlayerState
    {
        public MixerTransition2DAsset.UnShared transition;
    
        private Vector2 currentMoveInput;
        private Vector2 smoothVelocity;

        private void OnEnable()
        {
            controller.animancer.Play(transition);
        }

        public override void UpdateState()
        {
            if (controller.input.MoveInput != Vector2.zero) 
            {
                controller.TrySetState(StateIndex.Run);
                transition.State.Parameter = controller.input.MoveInput;
                Debug.LogError(controller.input.MoveInput);
            }
        }
    }
}