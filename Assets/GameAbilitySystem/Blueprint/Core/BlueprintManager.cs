using System.Collections.Generic;
using FlowCanvas;
using UIFlow;
using UnityEngine;

namespace GameAbilitySystem
{
    public class BlueprintManager : Singleton<BlueprintManager>
    {
        private readonly Queue<BlueprintNode> nodes = new();
        private bool isInit = false;

        private GameObject nodeAsset;
        private Transform activeRoot;
        private Transform disableRoot;
        
        public void StartBlueprint(FlowScript script)
        {
            Initialize();
            var node = nodes.Count > 0 ? nodes.Dequeue() : Instantiate(nodeAsset).GetComponent<BlueprintNode>();
            node.transform.SetParent(activeRoot, false);
            node.StartBlueprint(script);
        }

        public void StopBlueprint(BlueprintNode node)
        {
            node.StopBlueprint();
            node.transform.SetParent(disableRoot, false);
            nodes.Enqueue(node);
        }

        private void Initialize()
        {
            if (isInit)
                return;
            isInit = true;
            nodeAsset = Resources.Load<GameObject>("BlueprintNode");
            activeRoot = new GameObject("Active").transform;
            disableRoot = new GameObject("Disable").transform;
            activeRoot.SetParent(gameObject.transform, false);
            disableRoot.SetParent(gameObject.transform, false);
            disableRoot.gameObject.SetActive(false);
        }
    }
}