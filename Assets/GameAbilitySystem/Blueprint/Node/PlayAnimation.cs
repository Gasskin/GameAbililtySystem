using Animancer;
using FlowCanvas;
using FlowCanvas.Nodes;
using GASExample;
using ParadoxNotion.Design;
using UnityEngine;

namespace GameAbilitySystem
{
    [Category("GameAbilitySystem")]
    public class PlayAnimation : FlowControlNode
    {
        private FlowOutput onEnd;
        private FlowOutput onEvent;
    
        private ValueInput<ClipTransitionAsset> clipTransitionAsset;
        private ValueInput<GameObject> owner;
        private string eventName;
        
        protected override void RegisterPorts()
        {
            onEnd = AddFlowOutput("OnEnd");
            onEvent = AddFlowOutput("OnEvent");
    
            clipTransitionAsset = AddValueInput<ClipTransitionAsset>("clipTransitionAsset");
            owner = AddValueInput<GameObject>("owner");
            AddFlowInput(" ", FlowIn);
            AddValueOutput("eventName", () => eventName);
        }
    
        private void FlowIn(Flow f)
        {
            if (owner.value.TryGetComponent(out AnimancerComponent animancer))
            {
                var state = animancer.Play(clipTransitionAsset.value);
                state.Events.OnEnd = () =>
                {
                    state.Events.OnEnd = null;
                    if (owner.value.TryGetComponent(out PlayerController controller))
                        controller.stateMachine.ForceSetDefaultState();
                    f.Call(onEnd);
                };
    
                for (int i = 0; i < state.Events.Count; i++)
                {
                    var currentEventName = state.Events.Names[i];
                    state.Events.SetCallback(i,(() =>
                    {
                        eventName = currentEventName;
                        f.Call(onEvent);
                    }));
                }
            }
            else
            {
                f.Break(this);
            }
        }
    }

}