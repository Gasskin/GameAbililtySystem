using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FlowCanvas;
using UIFlow;
using UnityEngine;

namespace GameAbilitySystem
{
    public class BlueprintManager : Singleton<BlueprintManager>
    {
        private const string NODE_NAME = "BlueprintNode";

        private Queue<BlueprintNode> nodes = new();
        private bool isInit = false;

        private GameObject nodeAsset;
        private Transform activeRoot;
        private Transform disableRoot;
        
        public void StartBlueprint(FlowScript script)
        {
            Initialize();
            var node = nodes.Count > 0 ? nodes.Dequeue() : Instantiate(nodeAsset).GetComponent<BlueprintNode>();
            node.transform.SetParent(activeRoot, false);
            node.StartGraph(script);
        }

        public void EndBlueprint(BlueprintNode node)
        {
            node.transform.SetParent(disableRoot, false);
            nodes.Enqueue(node);
        }

        private void Initialize()
        {
            if (isInit)
                return;
            isInit = true;
            nodeAsset = Resources.Load<GameObject>(NODE_NAME);
            activeRoot = new GameObject("Active").transform;
            disableRoot = new GameObject("Disable").transform;
            activeRoot.SetParent(gameObject.transform, false);
            disableRoot.SetParent(gameObject.transform, false);
            disableRoot.gameObject.SetActive(false);
        }
    }
}