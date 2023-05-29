﻿using System.Collections.Generic;
using System.Reflection;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using NodeCanvas.Framework;
using UnityEngine;
using System.Linq;

namespace FlowCanvas.Nodes
{
    ///Base class for MethodBase -based wrappers
    [DoNotList]
    [ParadoxNotion.Design.Icon(runtimeIconTypeCallback: nameof(GetRuntimeIconType))]
    abstract public class ReflectedMethodBaseNodeWrapper : FlowNode, IReflectedWrapper
    {

        ISerializedReflectedInfo IReflectedWrapper.GetSerializedInfo() { return serializedMethodBase; }
        public System.Type GetRuntimeIconType() { return method != null ? method.DeclaringType : null; }

        [SerializeField]
        protected bool _callable;
        [SerializeField]
        protected bool _exposeParams;
        [SerializeField]
        protected int _exposedParamsCount;

        abstract protected ISerializedMethodBaseInfo serializedMethodBase { get; }
        private MethodBase method { get { return serializedMethodBase != null ? serializedMethodBase.GetMethodBase() : null; } }


#if UNITY_EDITOR
        public override string description {
            get { return method != null ? DocsByReflection.GetMemberSummary(method) : "Missing Method"; }
        }
#endif

        public bool callable {
            get { return _callable; }
            set
            {
                if ( _callable != value ) {
                    _callable = value;
                    GatherPorts();
                }
            }
        }

        public bool exposeParams {
            get { return _exposeParams; }
            set
            {
                if ( _exposeParams != value ) {
                    _exposeParams = value;
                    _exposedParamsCount = Mathf.Max(_exposedParamsCount, 1);
                    GatherPorts();
                }
            }
        }

        public int exposedParamsCount {
            get { return _exposedParamsCount; }
            set
            {
                if ( _exposedParamsCount != value ) {
                    _exposedParamsCount = value;
                    if ( _exposedParamsCount <= 0 ) {
                        _exposeParams = false;
                    }
                    GatherPorts();
                }
            }
        }

        ///Set a new method base info
        abstract public void SetMethodBase(MethodBase newMethod, object instance = null);


        public void SetDefaultParameterValues(MethodBase newMethod) {
            //set default parameter values
            var parameters = newMethod.GetParameters();
            for ( var i = 0; i < parameters.Length; i++ ) {
                var parameter = parameters[i];
                if ( parameter.IsOptional && parameter.DefaultValue != null ) {
                    var nameID = parameters[i].Name;
                    var paramPort = GetInputPort(nameID) as ValueInput;
                    if ( paramPort != null ) {
                        paramPort.serializedValue = parameter.DefaultValue;
                    }
                }
            }
        }

        public void SetDropInstanceReference(MethodBase newMethod, object instance = null) {
            //set possible instance reference
            if ( instance != null && !newMethod.IsStatic ) {
                var port = (ValueInput)GetFirstInputOfType(instance.GetType());
                if ( port != null ) {
                    port.serializedValue = instance;
                }
            }
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override UnityEditor.GenericMenu OnContextMenu(UnityEditor.GenericMenu menu) {
            menu = base.OnContextMenu(menu);
            if ( method != null ) {
                var overloads = new List<MethodBase>();
                if ( method is MethodInfo ) {
                    foreach ( var m in method.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public) ) {
                        if ( m.Name == method.Name ) {
                            overloads.Add(m);
                        }
                    }
                }
                if ( method is ConstructorInfo ) {
                    foreach ( var m in method.DeclaringType.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public) ) {
                        if ( m.Name == method.Name ) {
                            overloads.Add(m);
                        }
                    }
                }

                if ( overloads.Count > 1 ) {
                    foreach ( var _m in overloads ) {
                        var m = _m;
                        var isSame = m.SignatureName() == method.SignatureName();
                        menu.AddItem(new GUIContent("Change Method Overload/" + m.SignatureName()), isSame, () => { SetMethodBase(m); });
                    }
                }

                if ( method.IsGenericMethod && method is MethodInfo ) {
                    var methodInfo = (MethodInfo)method;
                    var arg1 = methodInfo.RTGetGenericArguments().FirstOrDefault();
                    foreach ( var _type in TypePrefs.GetPreferedTypesList(true) ) {
                        var type = _type;
                        var isSame = arg1 == type;
                        if ( methodInfo.TryMakeGeneric(type, out MethodInfo genericMethodInfo) ) {
                            menu.AddItem(new GUIContent("Change Generic Type/" + type.FriendlyName()), isSame, () => { SetMethodBase(genericMethodInfo); });
                        }
                    }
                }
            }
            return menu;
        }

        protected override void OnNodeInspectorGUI() {
            if ( method != null ) {
                System.Type returnType = null;
                if ( method is MethodInfo ) { returnType = ( method as MethodInfo ).ReturnType; }
                if ( method is ConstructorInfo ) { returnType = ( method as ConstructorInfo ).DeclaringType; }
                if ( returnType != typeof(void) && !method.Name.StartsWith("get_") ) {
                    callable = UnityEditor.EditorGUILayout.Toggle(EditorUtils.GetTempContent("Callable", null, "Enable this to explicitely call the node."), callable);
                }

                var parameters = method.GetParameters();
                var lastParam = parameters.LastOrDefault();
                if ( lastParam != null && lastParam.IsParams(parameters) ) {
                    exposeParams = UnityEditor.EditorGUILayout.Toggle(EditorUtils.GetTempContent("Expose Parameters", null, "This method accepts a params array. You can chose to expose the array to individual ports."), exposeParams);
                    if ( exposeParams ) {
                        UnityEditor.EditorGUI.indentLevel++;
                        exposedParamsCount = UnityEditor.EditorGUILayout.DelayedIntField("Parameters Count", exposedParamsCount);
                        UnityEditor.EditorGUI.indentLevel--;
                    }
                }
            }

            if ( method == null && serializedMethodBase != null ) {
                GUILayout.Label(serializedMethodBase.AsString());
            }

            base.OnNodeInspectorGUI();

            /*
            if (method != null){
                var xmlDoc = DocsByReflection.GetXmlElementForMember(method);
                if (xmlDoc != null){
                    EditorUtils.BoldSeparator();
                    foreach(var parameter in method.GetParameters()){
                        var doc = DocsByReflection.GetMethodParameter(method, parameter);
                        if (string.IsNullOrEmpty(doc)){
                            continue;
                        }
                        // var icon = UserTypePrefs.GetTypeIcon(parameter.ParameterType);
                        var color = UserTypePrefs.GetTypeColor(parameter.ParameterType);
                        var hexColor = Colors.ColorToHex(color);
                        var text = string.Format("<color={0}><b>{1}</b></color>\n {2}", hexColor, parameter.Name.SplitCamelCase(), doc);
                        var content = EditorUtils.GetTempContent(text);
                        GUILayout.Label(content);
                    }
                }
            }
            */

        }

#endif
        ///----------------------------------------------------------------------------------------------

    }
}