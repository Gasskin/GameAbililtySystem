using System.Collections;
using System.Collections.Generic;
using Animancer.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestScene : SceneView
{
    private static Texture _Icon;

    /// <summary>The icon image used by this window.</summary>
    public static Texture Icon
    {
        get
        {
            if (_Icon == null)
            {
                var name = EditorGUIUtility.isProSkin ? "ViewToolOrbit On" : "ViewToolOrbit";

                _Icon = AnimancerGUI.LoadIcon(name);
                if (_Icon == null)
                    _Icon = EditorGUIUtility.whiteTexture;
            }

            return _Icon;
        }
    }
    
    private GameObject root;
    
    public override void OnEnable()
    {
        base.OnEnable();
        titleContent = new GUIContent("Transition Preview",_Icon);

        var asset = Resources.Load<GameObject>("Cube");
        root = Instantiate(asset);
    }

    public override void OnDisable()
    {
        DestroyImmediate(root);
    }

    public static void GetWnd()
    {
        GetWindow<TestScene>(typeof(SceneView));
    }

    protected override void OnSceneGUI()
    {
        // base.OnSceneGUI();
    }
}
