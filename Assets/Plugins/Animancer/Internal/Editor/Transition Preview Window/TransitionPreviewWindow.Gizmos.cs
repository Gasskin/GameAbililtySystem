using System;
using UnityEngine;

namespace Animancer.Editor
{
    partial class TransitionPreviewWindow
    {
        [Serializable]
        public class Gizmos
        {
            public void DrawGizmos()
            {
                UnityEngine.Gizmos.DrawCube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            }
        }
    }
}