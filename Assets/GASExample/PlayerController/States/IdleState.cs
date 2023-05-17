using Animancer;

namespace GASExample
{
    public class IdleState : PlayerState
    {
        public ClipTransition clipTransition;

        private void OnEnable()
        {
            controller.animancer.Play(clipTransition);
        }
    }
}
