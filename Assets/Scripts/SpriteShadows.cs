using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadows : MonoBehaviour {

    [SerializeField]
    private UnityEngine.Rendering.ShadowCastingMode CastShadows;

    [SerializeField]
    private bool ReceiveShadows;

    void Awake() {
        var rend = GetComponent<Renderer>();
        rend.shadowCastingMode = CastShadows;
        rend.receiveShadows = ReceiveShadows;

    }
}
