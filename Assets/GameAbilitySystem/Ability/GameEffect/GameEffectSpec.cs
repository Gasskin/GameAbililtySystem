using System;
using GameAbilitySystem.Ability;

namespace GameAbilitySystem
{
    public class GameEffectSpec
    {
        public GameAttributeValue sourceCapturedAttributeValue;
        public GameAttributeValue targetCapturedAttributeValue;
        public AbilitySystemComponent source;
        public AbilitySystemComponent target;
        public GameEffect gameEffect;

        public float level;
        public float durationRemaining;
        public float totalDuration;
        public float timeUntilPeriodTick;

        private GameEffectSpec(GameEffect gameEffect, AbilitySystemComponent source, float level)
        {
            this.gameEffect = gameEffect;
            this.source = source;
            this.level = level;

            foreach (var modifier in this.gameEffect.modifiers)
            {
                modifier.modifierMagnitude.Initialise(this);
            }

            var durationMagnitude = this.gameEffect.durationMagnitude;
            if (durationMagnitude != null)
            {
                durationRemaining = durationMagnitude.CalculateMagnitude(this) * this.gameEffect.durationMultiplier;
                totalDuration = durationRemaining;
            }

            timeUntilPeriodTick = this.gameEffect.period;
            if (this.gameEffect.executeImmediate)
                timeUntilPeriodTick = 0;
        }

        public static GameEffectSpec CreateNew(GameEffect gameEffect, AbilitySystemComponent source, float Level = 1)
        {
            return new GameEffectSpec(gameEffect, source, Level);
        }


        public void UpdateRemainingDuration(float deltaTime)
        {
            if (gameEffect.durationPolicy == EDurationPolicy.Infinite)
                durationRemaining = 1;
            else
                durationRemaining -= deltaTime;
        }

        public void TickPeriod(float deltaTime, out bool executePeriodicTick)
        {
            executePeriodicTick = false;
            timeUntilPeriodTick -= deltaTime;
            if (timeUntilPeriodTick <= 0)
            {
                timeUntilPeriodTick = gameEffect.period;
                if (gameEffect.period > 0)
                    executePeriodicTick = true;
            }
        }
    }
}