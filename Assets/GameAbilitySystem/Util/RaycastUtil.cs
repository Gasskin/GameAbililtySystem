using System.Collections;
using System.Collections.Generic;
using GameAbilitySystem.Ability;
using UnityEngine;

namespace GameAbilitySystem
{
    public static class RaycastUtil
    {
        private static readonly Collider[] result1 = new Collider[1];
        private static readonly Collider[] result2 = new Collider[2];
        private static readonly Collider[] result3 = new Collider[3];
        private static readonly Collider[] result4 = new Collider[4];
        private static readonly Collider[] result5 = new Collider[5];
        private static readonly Collider[] result6 = new Collider[6];
        private static readonly Collider[] result7 = new Collider[7];
        private static readonly Collider[] result8 = new Collider[8];
        private static readonly Collider[] result9 = new Collider[9];
        private static readonly Collider[] result10 = new Collider[10];

        private static readonly List<AbilitySystemComponent> target = new();

        public static List<AbilitySystemComponent> SphereCast(Vector3 pos, float radius, int count = 1, int layMask = 0)
        {
            var results = GetPreferredResultContainer(count);
            var size = Physics.OverlapSphereNonAlloc(pos, radius, results, layMask);
            target.Clear();
            for (int i = 0; i < size; i++)
            {
                if (results[i].gameObject.TryGetComponent<AbilitySystemComponent>(out var comp))
                {
                    target.Add(comp);
                }
            }
            return target;
        }


        private static Collider[] GetPreferredResultContainer(int count = 1)
        {
            switch (count)
            {
                case 1:
                    return result1;
                case 2:
                    return result2;
                case 3:
                    return result3;
                case 4:
                    return result4;
                case 5:
                    return result5;
                case 6:
                    return result6;
                case 7:
                    return result7;
                case 8:
                    return result8;
                case 9:
                    return result9;
                case 10:
                    return result10;
                default:
                    return result10;
            }

            return null;
        }
    }
}