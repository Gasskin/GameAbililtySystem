using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

namespace GameAbilitySystem
{
    [Category("GameAbilitySystem")]
    public class EndAbility : FlowControlNode
    {
        private ValueInput<GameAbilitySpec> spec;
        
        protected override void RegisterPorts()
        {
            spec = AddValueInput<GameAbilitySpec>("AbilitySpec");
            
            AddFlowInput(" ", (f) =>
            {
                spec.value.EndAbility();
            });
        }
    }
}
