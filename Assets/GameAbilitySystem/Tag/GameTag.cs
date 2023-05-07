using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    [CreateAssetMenu(menuName = "GameAbilitySystem/Tag")]
    public class GameTag : ScriptableObject
    {
        [SerializeField]
        [LabelText("父标签")]
        [LabelWidth(40)]
        private GameTag parent;
        
        /// <summary>
        /// 是否是另一个标签的孩子
        /// </summary>
        /// <param name="other">另一个标签</param>
        /// <param name="depth">深度</param>
        /// <returns></returns>
        public bool IsDescendantOf(GameTag other, int depth = 8)
        {
            int i = 0;
            var tag = parent;
            while (depth > i++)
            {
                if (!tag) 
                    return false;

                if (tag == other) 
                    return true;

                tag = tag.parent;
            }
            return false;
        }
    }
}