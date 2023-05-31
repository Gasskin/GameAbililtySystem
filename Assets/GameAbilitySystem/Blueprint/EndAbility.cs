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
            AddValueInput<int>("Milliseconds");
            
            AddFlowInput(" ", (f) =>
            {
                Debug.LogError("结束啦");
                var ownerGo = flowGraph.agent.gameObject;
                if (ownerGo.TryGetComponent(out BlueprintNode node))
                {
                    BlueprintManager.Instance.StopBlueprint(node);
                }
            });
        }
    }
}
