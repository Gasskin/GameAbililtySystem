using Cysharp.Threading.Tasks;
using GameAbilitySystem.Ability;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public AbilitySystemComponent source;
    public AbilitySystemComponent target;

    public Vector3 speed;

    public async UniTask TravelToTarget()
    {
        while (Vector3.Distance(target.transform.position, transform.position) > 0.2)
        {
            var position = transform.position;
            var dir = (target.transform.position - position).normalized;
            position += Vector3.Scale(dir,speed) * Time.deltaTime;
            transform.position = position;
            await UniTask.Yield();
        }

        Debug.LogError("到达目标");
    }
}