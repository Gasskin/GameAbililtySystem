using FlowCanvas;
using FlowCanvas.Nodes;
using GameAbilitySystem.Ability;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;

namespace GameAbilitySystem
{
    [Category("GameAbilitySystem")]
    public class OnAbilityStart : RouterEventNode<GraphOwner>
    {
        private FlowOutput onEnter;
        private GameAbilitySpec spec;
        private GameObject owner;

        protected override void RegisterPorts()
        {
            onEnter = AddFlowOutput(" ");
            AddValueOutput("AbilitySpec", () => spec);
            AddValueOutput("Owner", () => owner);
        }

        protected override void Subscribe(EventRouter router)
        {
            router.onCustomEvent += OnAbilityStartAction;
        }

        protected override void UnSubscribe(EventRouter router)
        {
            router.onCustomEvent -= OnAbilityStartAction;
        }

        private void OnAbilityStartAction(string eventName, IEventData data)
        {
            if (data is EventData<GameAbilitySpec> eventData)
            {
                owner = eventData.sender as GameObject;
                spec = eventData.value;
                onEnter.Call(new Flow());
            }
        }
    }
}