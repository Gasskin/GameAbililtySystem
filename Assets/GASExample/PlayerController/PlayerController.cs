using Animancer;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public struct StateIndex
{
    public const int Idle = 0;

    public const int Walk = 1;
}

public class PlayerController : MonoBehaviour
{
    public PlayerState.StateMachine stateMachine;
    public AnimancerComponent animancer;
    public PlayerState[] states;

    public float moveInputSmoothSpeed = 0.1f;


    [DoNotSerialize,HideInInspector]
    public Vector2 moveInput;
#region 生命周期
    private void Awake()
    {
        stateMachine.InitializeAfterDeserialize();
    }

    private void Update()
    {
        UpdateInput();
    }
#endregion

#region 输入控制
    private void UpdateInput()
    {
    }
#endregion

#region Input
    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        if (ctx.performed)
            stateMachine.TrySetState(states[StateIndex.Walk]);
        else
            stateMachine.TrySetDefaultState();
    }

    public void Skill_1(InputAction.CallbackContext ctx)
    {
    }

    public void Skill_2(InputAction.CallbackContext ctx)
    {
    }

    public void Skill_3(InputAction.CallbackContext ctx)
    {
    }

    public void Skill_4(InputAction.CallbackContext ctx)
    {
    }
#endregion
}