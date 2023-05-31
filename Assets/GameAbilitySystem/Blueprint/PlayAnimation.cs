using Animancer;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;


[Category("Custom")]
public class PlayAnimation : FlowControlNode 
{
    protected override void RegisterPorts()
    {
        var condition = AddValueInput<bool>("Condition");
        var trueOut = AddFlowOutput("OnEnd");
        var falseOut = AddFlowOutput("OnEvent");

        var animation = AddValueInput<AnimationClip>("clip");
        var animancer = AddValueInput<AnimancerComponent>("Animancer");
 
        AddFlowInput("In", (f) =>
        {
            var state = animancer.value.Play(animation.value);
            state.Events.OnEnd = () =>
            {
                f.Call(condition.value ? trueOut : falseOut);
                state.Events.OnEnd = null;
            };
        } );
    }
}
