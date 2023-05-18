using Animancer;
using GameAbilitySystem;

namespace GASExample
{
    public class AttackState : PlayerState
    {
        public ClipTransition transition;

        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.animancer.Play(transition);
        }

        public override void PrepareState(BaseAbilitySpec spec)
        {
            if (spec is ComboAbility.ComboAbilitySpec cba)
            {
                transition.Speed= cba.speedValue;
                transition = cba.transition;
                transition.Events.SetCallback("Start", cba.OnStart);
                transition.Events.SetCallback("HitPoint", cba.OnHitPoint);
                transition.Events.SetCallback("BackSwing", cba.OnBackSwing);
                transition.Events.OnEnd = (() =>
                {
                    cba.OnEnd();
                    controller.stateMachine.ForceSetDefaultState();
                });
            }
        }
    }
}