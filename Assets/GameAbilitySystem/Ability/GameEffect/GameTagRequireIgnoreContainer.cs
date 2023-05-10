using System;
using Sirenix.OdinInspector;

namespace GameAbilitySystem
{
    [Serializable]
    public struct GameTagRequireIgnoreContainer
    {
        [LabelText("必须包含")]
        public GameTag[] requireTags;
        [LabelText("禁止包含")]
        public GameTag[] ignoreTags;
    }
}