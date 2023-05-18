using Animancer;
using Animancer.FSM;
using GameAbilitySystem;
using Sirenix.OdinInspector;

namespace GASExample
{

    public enum PlayerStatePriority
    {
        Basic = 0,
        Attack = 2,
        Ability = 3,
        Hit = 4,
    }

    public abstract class PlayerState : StateBehaviour
    {
        [System.Serializable]
        public class StateMachine : StateMachine<PlayerState>.WithDefault
        {
        }

        public PlayerController controller;
        [LabelText("权重类型")]
        public PlayerStatePriority priority = PlayerStatePriority.Basic;
        [LabelText("能否切换到自身")]
        public bool canInterruptSelf = false;

        public override bool CanExitState
        {
            get
            {
                var nextState = controller.stateMachine.NextState;
                if (nextState == this)
                    return canInterruptSelf;
                if (priority == PlayerStatePriority.Basic)
                    return true;
                return nextState.priority > priority;
            }
        }

        public virtual void PrepareState(BaseAbilitySpec spec)
        {
            
        }
        

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.GetComponentInParentOrChildren(ref controller);
        }
#endif
    }
}