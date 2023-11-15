using Animancer;
using Cysharp.Threading.Tasks;
using FlowCanvas;
using GameAbilitySystem;
using GameAbilitySystem.Ability;
using GASExample;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "GASExample/Ability/Combo Ability")]
public class ComboAbility : GameAbility
{
    [LabelText("前摇效果")]
    public GameEffect startEffect;
    [LabelText("命中效果")]
    public GameEffect hitPointEffect;
    [LabelText("后摇效果")]
    public GameEffect backSwingEffect;
    [LabelText("速度")]
    public GameAttribute speed;

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
    
    
    public override GameAbilitySpec CreateSpec(AbilitySystemComponent owner,FlowScriptController blueprintController)
    {
        var spec = new ComboAbilitySpec(this, owner)
        {
            level = owner.level,
            speed = speed,
        };
        return spec;
    }

    public class ComboAbilitySpec : GameAbilitySpec
    {
        public ClipTransition transition;
        public PlayerController controller;
        public GameAttribute speed;
        public float speedValue;
        private ComboAbility comboAbility;
        
        public ComboAbilitySpec(GameAbility ability, AbilitySystemComponent owner) : base(ability, owner)
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

        public override void ActivateAbility()
        {
            base.ActivateAbility();
            transition = comboAbility.clips[comboAbility.ComboIndex];
            owner.attributeSystemComponent.TryGetAttributeValue(speed, out var value);
            speedValue = value.currentValue;
            controller.PrepareAttack(this);
            controller.EnterAttack();
        }
        
        public override void EndAbility()
        {
            if (!isActive)
                return;
            base.EndAbility();
            comboAbility.ComboIndex = 0;
            var spec = owner.MakeGameEffectSpec(comboAbility.backSwingEffect, level);
            owner.ApplyGameEffectSpecToSelf(spec);
        }

    #region 动画监听

        public void OnStart()
        {
            var spec = owner.MakeGameEffectSpec(comboAbility.startEffect, level);
            owner.ApplyGameEffectSpecToSelf(spec);
        }
        
        public void OnHitPoint()
        {
            comboAbility.ComboIndex++;
        }
        
        public void OnBackSwing()
        {
            var spec = owner.MakeGameEffectSpec(comboAbility.backSwingEffect, level);
            owner.ApplyGameEffectSpecToSelf(spec);
        }

        public void OnEnd()
        {
            EndAbility();
        }
    #endregion
    }
}