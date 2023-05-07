using System.Collections;
using System.Collections.Generic;
using GameAbilitySystem;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameTag father;
    public GameTag child;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(child.IsDescendantOf(father));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
