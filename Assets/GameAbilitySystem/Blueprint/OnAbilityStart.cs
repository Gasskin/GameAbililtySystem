using FlowCanvas;
using FlowCanvas.Nodes;
using GameAbilitySystem.Ability;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Services;

namespace GameAbilitySystem
{
    [Category("GameAbilitySystem")]
    public class OnAbilityStart : RouterEventNode<GraphOwner>
    {
        private FlowOutput onEnter;
        private AbilitySystemComponent asc;

        protected override void RegisterPorts()
        {
            onEnter = AddFlowOutput(" ");
            AddValueOutput("AbilitySystemComponent", () => asc);
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
            onEnter.Call(new Flow());
        }
    }
}