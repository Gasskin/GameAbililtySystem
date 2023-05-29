﻿using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Macros
{

    [DoNotList]
    [ParadoxNotion.Design.Icon("MacroIn")]
    [Description("Defines the Input ports of the Macro.\nTo quickly create ports, you can also Drag&Drop a connection on top of this node!")]
    [ProtectedSingleton]
    public class MacroInputNode : FlowNode
    {

        public override ParadoxNotion.Alignment2x2 iconAlignment {
            get { return ParadoxNotion.Alignment2x2.Default; }
        }

        private Macro macro {
            get { return graph as Macro; }
        }

        protected override void RegisterPorts() {
            if ( macro == null || macro.inputDefinitions == null ) { return; }
            for ( var i = 0; i < macro.inputDefinitions.Count; i++ ) {
                var def = macro.inputDefinitions[i];
                if ( def.type == typeof(Flow) ) {
                    macro.entryActionMap[def.ID] = AddFlowOutput(def.name, def.ID).Call;
                } else {
                    AddValueOutput(def.name, def.type, () => { return macro.entryFunctionMap[def.ID](); }, def.ID);
                }
            }
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override UnityEditor.GenericMenu OnContextMenu(UnityEditor.GenericMenu menu) {
            return null;
        }

        protected override UnityEditor.GenericMenu OnDragAndDropPortContextMenu(UnityEditor.GenericMenu menu, Port port) {
            if ( macro == null ) { return menu; }
            if ( port.IsInputPort() ) {
                menu.AddItem(new GUIContent(string.Format("Promote to new Input '{0}'", port.name)), false, () =>
                {
                    var def = new DynamicParameterDefinition(port.name, port.type);
                    var defPort = macro.AddInputDefinition(def);
                    BinderConnection.Create(defPort, port);
                });
            }
            return menu;
        }

        protected override void OnNodeInspectorGUI() {
            if ( macro == null ) { return; }
            if ( GUILayout.Button("Add Flow Input") ) {
                macro.AddInputDefinition(new DynamicParameterDefinition("Flow Input", typeof(Flow)));
            }

            if ( GUILayout.Button("Add Value Input") ) {
                EditorUtils.ShowPreferedTypesSelectionMenu(typeof(object), (t) =>
                {
                    macro.AddInputDefinition(new DynamicParameterDefinition(string.Format("{0} Input", t.FriendlyName()), t));
                });
            }

            var options = new EditorUtils.ReorderableListOptions();
            options.allowRemove = true;
            EditorUtils.ReorderableList(macro.inputDefinitions, options, (i, picked) =>
            {
                var def = macro.inputDefinitions[i];
                GUILayout.BeginHorizontal();
                def.name = UnityEditor.EditorGUILayout.DelayedTextField(def.name, GUILayout.Width(150), GUILayout.ExpandWidth(true));
                GUI.enabled = def.type != typeof(Flow);
                EditorUtils.ButtonTypePopup("", def.type, (t) => { def.type = t; GatherPorts(); });
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            });

            if ( GUI.changed ) { GatherPorts(); }
        }

#endif

    }
}