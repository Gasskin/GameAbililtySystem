using Animancer;
using GameAbilitySystem;

namespace GASExample
{
    public class AbilityState : PlayerState
    {
        public ClipTransition transition;


        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.animancer.Play(transition);
        }

        public override void PrepareState(BaseAbilitySpec spec)
        {
            // if (spec is SimpleAbility.SimpleAbilitySpec simpleAbilitySpec)
            // {
            //     transition = simpleAbilitySpec.transition;
            //     if (transition.Events.IndexOf("Start") >= 0) 
            //         transition.Events.SetCallback("Start",simpleAbilitySpec.OnStart);
            //     if (transition.Events.IndexOf("HitPoint") >= 0) 
            //         transition.Events.SetCallback("HitPoint",simpleAbilitySpec.OnHitPoint);
            //     if (transition.Events.IndexOf("BackSwing") >= 0) 
            //         transition.Events.SetCallback("BackSwing",simpleAbilitySpec.OnBackSwing);
            //     transition.Events.OnEnd = (() =>
            //     {
            //         simpleAbilitySpec.OnEnd();
            //         controller.stateMachine.ForceSetDefaultState();
            //     });
            // }
        }
    }
}
