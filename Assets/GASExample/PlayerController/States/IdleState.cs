using Animancer;

namespace GASExample
{
    public class IdleState : PlayerState
    {
        public ClipTransition clipTransition;

        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.animancer.Play(clipTransition);
        }
    }
}
