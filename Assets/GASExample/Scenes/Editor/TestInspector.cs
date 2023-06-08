using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.GettingStarted;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Test))]
public class TestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("打开"))
        {
            TestScene.GetWnd();
        }
    }
}
