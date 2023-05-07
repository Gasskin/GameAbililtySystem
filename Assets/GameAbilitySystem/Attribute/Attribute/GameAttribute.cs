using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAbilitySystem
{
    [CreateAssetMenu(menuName = "GameAbilitySystem/Attribute")]
    public class GameAttribute : ScriptableObject
    {
        [LabelText("属性名"), DelayedProperty] [OnValueChanged("OnNameChanged")] [LabelWidth(50)]
        public string name;

        public virtual AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue,
            List<AttributeValue> allAttributeValues)
        {
            attributeValue.currentValue = (attributeValue.baseValue + attributeValue.modifier.add) *
                                          (attributeValue.modifier.multiply + 1);

            if (attributeValue.modifier.overwrite != 0)
            {
                attributeValue.currentValue = attributeValue.modifier.overwrite;
            }

            return attributeValue;
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