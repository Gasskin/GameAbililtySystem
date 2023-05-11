// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2023 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;
using UnityEngine.UI;

namespace Animancer.Examples.AnimatorControllers.GameKit
{
    /// <summary>A simple system for selecting characters.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/animator-controllers/3d-game-kit">3D Game Kit</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.AnimatorControllers.GameKit/CharacterSelector
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Game Kit - Character Selector")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(AnimatorControllers) + "." + nameof(GameKit) + "/" + nameof(CharacterSelector))]
    public sealed class CharacterSelector : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Text _Text;
        [SerializeField] private GameObject[] _Characters;

        /************************************************************************************************************************/

        private void Awake()
        {
            SelectCharacter(0);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            if (ExampleInput.Number1Up)
                SelectCharacter(0);
            else if (ExampleInput.Number2Up)
                SelectCharacter(1);
        }

        /************************************************************************************************************************/

        private void SelectCharacter(int index)
        {
            var text = ObjectPool.AcquireStringBuilder();

            for (int i = 0; i < _Characters.Length; i++)
            {
                // Activate the target and deactivate everyone else.
                var active = i == index;
                _Characters[i].SetActive(active);

                // Build all their names into a string explaining which keys they correspond to.

                if (i > 0)
                    text.AppendLine();

                if (active)// Use Rich Text Tags to make the active character Bold.
                    text.Append("<b>");

                text.Append(1 + i)
                    .Append(" = ")
                    .Append(_Characters[i].name);

                if (active)
                    text.Append("</b>");
            }

            _Text.text = text.ReleaseToString();
        }

        /************************************************************************************************************************/
    }
}
