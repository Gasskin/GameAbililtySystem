using Animancer;
using UnityEngine;

namespace GASExample
{
    public class IdleState : PlayerState
    {
        public ClipTransition clipTransition;

        public override void UpdateState()
        {
            if (controller.input.MoveInput == Vector2.zero)
            {
                controller.TrySetState(StateIndex.Idle);
            }
        }

        private void OnEnable()
        {
            controller.animancer.Play(clipTransition);
        }
    }
}
