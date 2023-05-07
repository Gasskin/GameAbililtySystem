using System;
using Sirenix.OdinInspector;

namespace GameplayAbilitySystem
{
    [Serializable]
    public struct GameTagRequireIgnoreContainer
    {
        [LabelText("需要"), LabelWidth(50)]
        public GameTag[] require;
        [LabelText("忽略"),LabelWidth(50)]
        public GameTag[] ignore;
    }
}