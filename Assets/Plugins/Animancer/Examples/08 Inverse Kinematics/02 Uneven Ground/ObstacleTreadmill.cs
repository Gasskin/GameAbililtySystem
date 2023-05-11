// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2023 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Examples.InverseKinematics
{
    /// <summary>Spawns a bunch of obstacles and randomises them each time the target moves too far away.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/ik/uneven-ground">Uneven Ground</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.InverseKinematics/ObstacleTreadmill
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Inverse Kinematics - Obstacle Treadmill")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(InverseKinematics) + "/" + nameof(ObstacleTreadmill))]
    public sealed class ObstacleTreadmill : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private float _SpawnCount = 10;
        [SerializeField] private Material _ObstacleMaterial;
        [SerializeField, Meters] private float _Length;
        [SerializeField, Degrees] private float _RotationVariance = 45;
        [SerializeField, Multiplier] private float _BaseScale = 1;
        [SerializeField, Multiplier] private float _ScaleVariance = 0.1f;
        [SerializeField] private Transform _Target;

        /************************************************************************************************************************/

        private readonly List<Transform> Obstacles = new List<Transform>();

        /************************************************************************************************************************/

        private void Awake()
        {
            // Spawn a bunch of obstacles and randomize their layout.
            for (int i = 0; i < _SpawnCount; i++)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;
                obj.GetComponent<Renderer>().sharedMaterial = _ObstacleMaterial;
                obj.parent = transform;
                Obstacles.Add(obj);
            }

            ScrambleObjects();
        }

        /************************************************************************************************************************/

        private void ScrambleObjects()
        {
            // Move and rotate each of the obstacles randomly.
            for (int i = 0; i < Obstacles.Count; i++)
            {
                var obj = Obstacles[i];
                obj.localPosition = new Vector3(Random.Range(0, _Length), 0, 0);
                obj.localRotation = Quaternion.Euler(90, Random.Range(-_RotationVariance, _RotationVariance), 0);
                obj.localScale = Vector3.one * (_BaseScale + Random.Range(-_ScaleVariance, _ScaleVariance));
            }
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            // When the target moves too far, teleport them back and randomize the obstacles again.
            var position = _Target.position;
            if (position.x < transform.position.x)
            {
                ScrambleObjects();

                position.x += _Length;

                // Adjust the height to make sure it's above the ground.
                position.y += 5;
                if (Physics.Raycast(position, Vector3.down, out var raycastHit, 10))
                    position = raycastHit.point;

                _Target.position = position;
            }
        }

        /************************************************************************************************************************/

        [SerializeField]
        private Transform _Ground;

        // Set by a UI Slider.
        public float Slope
        {
            get => _Ground.localEulerAngles.z;
            set => _Ground.localEulerAngles = new Vector3(0, 0, value);
        }

        /************************************************************************************************************************/
    }
}
