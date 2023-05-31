
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using UnityEngine;

[Category("Custom")]
public class LogValueTest : CallableActionNode<string>
{
    public override void Invoke(string a)
    {
        Debug.LogError($"自定义节点:{a}");
    }
}
