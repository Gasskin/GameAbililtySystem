using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

namespace GameAbilitySystem
{
    [Category("GameAbilitySystem")]
    public class EndAbility : FlowControlNode
    {
        private ValueInput<BaseAbilitySpec> spec;
        
        protected override void RegisterPorts()
        {
            spec = AddValueInput<BaseAbilitySpec>("AbilitySpec");
            
            AddFlowInput(" ", (f) =>
            {
                spec.value.EndAbility();
            });
        }
    }
}
