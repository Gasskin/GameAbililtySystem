using Animancer;
using UnityEngine;

namespace GASExample
{
    public struct StateIndex
    {
        public const int Idle = 0;

        public const int Run = 1;

        public const int Attack = 2;
    }

    public class PlayerController : MonoBehaviour
    {
        public PlayerState.StateMachine stateMachine;
        public AnimancerComponent animancer;
        public PlayerState[] states;
        public PlayerInput input = new();

        private void Awake()
        {
            stateMachine.InitializeAfterDeserialize();
        }

        private void Update()
        {
            input.ResetInput();
            input.UpdateInput();

            foreach (var state in states)
                state.UpdateState();
        }

        public void TrySetState(int index)
        {
            if (index >= states.Length)
                return;
            stateMachine.TrySetState(states[index]);
        }

        public void ForceSetState(int index)
        {
            if (index >= states.Length)
                return;
            stateMachine.ForceSetState(states[index]);
        }

        public void TrySetDefaultState()
        {
            stateMachine.TrySetDefaultState();
        }
    }
}