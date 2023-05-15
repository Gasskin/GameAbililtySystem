using Animancer;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public AnimancerComponent animancer;
    public ClipTransition idle;
    public ClipTransition move;

    public BaseAbility ability;
    public AbilitySystemComponent asc;

    private Vector3 targetPos = Vector3.zero;

    private void Start()
    {
        animancer.Play(idle);
    }

    void Update()
    {
        MoveToTarget();
    }

    public void PlayAnimation(ClipTransition clip)
    {
        var state = animancer.Play(clip);
        state.Events.OnEnd = () => { animancer.Play(idle); };
    }
    
#region Input
    public void Move(InputAction.CallbackContext ctx)
    {
        if (Camera.main == null)
            return;
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var info, 500, 1 << LayerMask.NameToLayer("Ground")))
            return;
        targetPos = info.point;
        targetPos.y = 0;
        var direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 90 - angle, 0);
        animancer.Play(move);
    }

    public void Skill_1(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            targetPos = Vector3.zero;
            var spec = ability.CreateSpec(asc);
            spec.TryActivateAbility();
        }
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

    private void MoveToTarget()
    {
        if (targetPos == Vector3.zero)
            return;
        var dir = targetPos - transform.position;
        var len = dir.magnitude;
        var moveDir = dir.normalized * speed * Time.deltaTime;
        var moveLen = moveDir.magnitude;
        if (moveLen >= len)
        {
            transform.parent.position = targetPos;
            targetPos = Vector3.zero;
            animancer.Play(idle);
        }
        else
        {
            transform.parent.position += moveDir;
        }
    }
#endregion
}