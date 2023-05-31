using FlowCanvas;
using UnityEngine;

namespace GameAbilitySystem
{
    public class BlueprintNode : MonoBehaviour
    {
        public FlowScriptController controller;

        public void StartBlueprint(FlowScript script)
        {
            controller.graph = script;
            controller.StartBehaviour();
            controller.SendEvent("OnAbilityStart");
        }

        public void StopBlueprint()
        {
            controller.graph = null;
            controller.StopBehaviour();
        }
    }
}

