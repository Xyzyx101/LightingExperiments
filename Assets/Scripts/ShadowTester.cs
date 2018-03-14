using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTester : MonoBehaviour {
    
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        Renderer rend = GetComponent<Renderer>();
        var foo = rend.shadowCastingMode;
        Debug.Log(foo);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
    }
}
