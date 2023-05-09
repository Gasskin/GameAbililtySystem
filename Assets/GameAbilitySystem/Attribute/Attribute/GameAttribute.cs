using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    /// <summary>
    /// 基础属性，不包含任何数据，仅仅是一个标记
    /// </summary>
    [CreateAssetMenu(menuName = "GameAbilitySystem/Attribute")]
    public class GameAttribute : ScriptableObject
    {
        [LabelText("属性名"), DelayedProperty] [OnValueChanged("OnNameChanged")] [LabelWidth(50)]
        public string name;

        public virtual GameAttributeValue CalculateCurrentAttributeValue(GameAttributeValue gameAttributeValue,
            List<GameAttributeValue> allAttributeValues)
        {
            gameAttributeValue.currentValue = (gameAttributeValue.baseValue + gameAttributeValue.modifier.add) *
                                          (gameAttributeValue.modifier.multiply + 1);

            if (gameAttributeValue.modifier.overwrite != 0)
            {
                gameAttributeValue.currentValue = gameAttributeValue.modifier.overwrite;
            }

            return gameAttributeValue;
        }

#if UNITY_EDITOR
        [PropertySpace]
        [Title("说明")]
        [InfoBox("这是一个基础属性，它的值由自身的基础值和修改器计算得出。\n" +
                 "自身属性 = 基础值 + 修改器")]
        [OnInspectorGUI]
        [HideIf("@this.GetType() != typeof(GameAttribute)")]
        private void OnInspectorGUI()
        {
        }

        private void OnNameChanged()
        {
            string[] guids = UnityEditor.Selection.assetGUIDs;
            int i = guids.Length;
            if (i == 1)
            {
                string guid = guids[0];
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var so = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                if (so != this)
                {
                    return;
                }

                var fileName = Path.GetFileNameWithoutExtension(assetPath);
                var newName = $"{name}";
                if (fileName != newName)
                {
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, newName);
                }
            }
        }
#endif
    }
}