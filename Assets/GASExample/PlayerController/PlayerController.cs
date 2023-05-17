using Animancer;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using UnityEngine;

namespace GASExample
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerState.StateMachine stateMachine;
        public PlayerInput input = new();
        public PlayerState idle;
        public PlayerState run;
        public PlayerState attack;
        public PlayerState ability;
        
        public AnimancerComponent animancer;

        public AbilitySystemComponent asc;
        public BaseAbility attackAbility;

        private void Awake()
        {
            stateMachine.InitializeAfterDeserialize();
        }

        private void Update()
        {
            input.ResetInput();
            input.GetInput();
            
            if (input.MoveInput == Vector2.zero)
            {
                stateMachine.TrySetState(idle);
            }
            else
            {
                stateMachine.TrySetState(run);
            }
            
            if (input.Attack)
            {
                var spec = attackAbility.CreateSpec(asc);
                spec.TryActivateAbility();
            }
        }

    }
}