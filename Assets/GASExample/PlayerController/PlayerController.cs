using System;
using Animancer;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using UnityEngine;

namespace GASExample
{
    public class PlayerController : MonoBehaviour
    {

        public PlayerState.StateMachine stateMachine;
        public PlayerState idle;
        public PlayerState run;
        public PlayerState attack;
        
        public AnimancerComponent animancer;
        public AbilitySystemComponent asc;
        
        public readonly PlayerInput input = new();

        public GameAbility attackAbility;
        public GameAbility initAbility;
        public GameAbility rollAbility;

        // private BaseAbilitySpec attackSpec;
        
        private void Awake()
        {
            stateMachine.InitializeAfterDeserialize();
        }

        private void Start()
        {
            asc.AddAbility(attackAbility);
            asc.AddAbility(initAbility);
            asc.AddAbility(rollAbility);
            asc.ActiveAbility(initAbility);

            // attackSpec = attackAbility.CreateSpec(asc);
        }

        private void Update()
        {
            input.ResetInput();
            input.GetInput();
            
            if (input.MoveInput == Vector2.zero)
            {
                stateMachine.TryResetState(idle);
            }
            else
            {
                stateMachine.TryResetState(run);
            }
            
            if (input.Attack)
            {
                asc.ActiveAbility(attackAbility);
            }

            if (input.Roll)
            {
                asc.ActiveAbility(rollAbility);
            }
        }

        public void PrepareAttack(GameAbilitySpec spec)
        {
            attack.PrepareState(spec);
        }

        public void EnterAttack()
        {
            stateMachine.TryResetState(attack);
        }
    }
}