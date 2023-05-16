using Animancer;
using Animancer.FSM;

public enum PlayerStatePriority
{
    Low = 0,
    Medium = 1,
    High = 2,
}

public class PlayerState : StateBehaviour
{
    [System.Serializable]
    public class StateMachine : StateMachine<PlayerState>.WithDefault
    {
    }

    public PlayerController controller;

    public virtual PlayerStatePriority Priority => PlayerStatePriority.Low;

    public virtual bool CanInterruptSelf => false;

    public override bool CanExitState
    {
        get
        {
            var nextState = controller.stateMachine.NextState;
            if (nextState == this)
                return CanInterruptSelf;
            if (Priority == PlayerStatePriority.Low)
                return true;
            return nextState.Priority > Priority;
        }
    }
    
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.GetComponentInParentOrChildren(ref controller);
    }
#endif
}