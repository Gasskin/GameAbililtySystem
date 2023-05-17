using Animancer;
using Cysharp.Threading.Tasks;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using GASExample;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GASExample/Ability/Combo Ability")]
public class ComboAbility : BaseAbility
{
    [LabelText("前摇效果")]
    public GameEffect startEffect;
    [LabelText("命中效果")]
    public GameEffect hitPointEffect;
    [LabelText("后摇效果")]
    public GameEffect backSwingEffect;
    
    [LabelText("连招")]
    [EventNames("Start", "HitPoint", "BackSwing")]
    public ClipTransition[] clips;

    private int comboIndex = 0;
    public int ComboIndex
    {
        get
        {
            if (comboIndex < clips.Length)
            {
                var copy = comboIndex;
                comboIndex++;
                return copy;
            }

            comboIndex = 0;
            return comboIndex;
        }
        set
        {
            value = Mathf.Clamp(value, 0, clips.Length - 1);
            comboIndex = value;
        }
    }
    
    
    public override BaseAbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        var spec = new ComboAbilitySpec(this, owner)
        {
            level = owner.level,
            transition = clips[ComboIndex],
        };
        return spec;
    }

    private class ComboAbilitySpec : BaseAbilitySpec
    {
        public ClipTransition transition;
        public PlayerController controller;

        private ComboAbility comboAbility;
        
        public ComboAbilitySpec(BaseAbility ability, AbilitySystemComponent owner) : base(ability, owner)
        {
            comboAbility = ability as ComboAbility;
            controller = owner.GetComponent<PlayerController>();
        }

        protected override bool CheckGameTags()
        {
            if (owner.HasAllTags(comboAbility.startEffect.grantedTags))
                return false;
            return owner.HasAllTags(ability.ownerTags.requireTags)
                   && owner.HasNoTags(ability.ownerTags.ignoreTags)
                   && owner.HasAllTags(ability.sourceTags.requireTags)
                   && owner.HasNoTags(ability.sourceTags.ignoreTags)
                   && owner.HasAllTags(ability.targetTags.requireTags)
                   && owner.HasNoTags(ability.targetTags.ignoreTags);
        }

        public override UniTask PreActivate()
        {
            transition.Events.SetCallback("Start",OnStart);
            transition.Events.SetCallback("HitPoint",OnHitPoint);
            transition.Events.SetCallback("BackSwing",OnBackSwing);
            transition.Events.OnEnd = OnEnd;
            return base.PreActivate();
        }

        public override UniTask ActivateAbility()
        {
            var state = controller.attack as AttackState;
            state.transition = transition;
            controller.stateMachine.ForceSetState(controller.attack);
            return base.ActivateAbility();
        }

        public override void EndAbility()
        {
            base.EndAbility();
        }


        private void OnStart()
        {
            var spec = owner.MakeGameEffectSpec(comboAbility.startEffect, level);
            owner.ApplyGameEffectSpecToSelf(spec);
        }
        
        private void OnHitPoint()
        {
            comboAbility.ComboIndex++;
        }
        
        private void OnBackSwing()
        {
            var spec = owner.MakeGameEffectSpec(comboAbility.backSwingEffect, level);
            owner.ApplyGameEffectSpecToSelf(spec);
        }

        private void OnEnd()
        {
            comboAbility.ComboIndex = 0;
            controller.stateMachine.ForceSetDefaultState();
        }
    }
}