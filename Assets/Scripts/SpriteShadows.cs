using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteShadows : MonoBehaviour {

    [SerializeField]
    private UnityEngine.Rendering.ShadowCastingMode CastShadows;

    void Awake() {
        var rend = GetComponent<Renderer>();
        rend.shadowCastingMode = CastShadows;
    }
}
