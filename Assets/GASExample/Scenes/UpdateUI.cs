using GameAbilitySystem;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public Image bg;
    public GameAttribute value;
    public GameAttribute maxValue;
    public AttributeSystemComponent attributeSystemComponent;

    void Update()
    {
        if (attributeSystemComponent.TryGetAttributeValue(value, out var current)
            && attributeSystemComponent.TryGetAttributeValue(maxValue, out var max))
        {
            bg.fillAmount = Mathf.Clamp(current.currentValue / max.currentValue, 0, 1);
        }
    }
}