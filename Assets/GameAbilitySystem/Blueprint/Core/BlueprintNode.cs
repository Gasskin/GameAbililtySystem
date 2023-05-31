using FlowCanvas;
using UnityEngine;

namespace GameAbilitySystem
{
    public class BlueprintNode : MonoBehaviour
    {
        public FlowScriptController controller;

        private const string START_FUNC = "ON_GRAPH_START";
        
        public void StartGraph(FlowScript script)
        {
            controller.graph = script;
            controller.StartBehaviour();
            // controller.CallFunction(START_FUNC);
            controller.SendEvent("OnAbilityStart");
        }
    }
}

