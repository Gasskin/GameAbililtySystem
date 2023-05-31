using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

namespace GameAbilitySystem
{
    [Category("GameAbilitySystem")]
    public class EndAbility : FlowControlNode 
    {
        protected override void RegisterPorts()
        {
            AddFlowInput(" ", (f) =>
            {
                Debug.LogError("结束啦！！");
            });
        }
    }
}
