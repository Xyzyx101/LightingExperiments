using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField]
    private float KickForce = 10.0f;
    private Rigidbody Body;
    private float Radius;

    private void Awake() {
        Body = GetComponent<Rigidbody>();
        var spheres = GetComponents<SphereCollider>();
        foreach( var sphere in spheres ) {
            if( !sphere.isTrigger ) {
                Radius = sphere.radius;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if( other.gameObject.name == "Player" ) {
            Kick(other);
        }
    }

    private void Kick(Collider playerCollider) {
        var otherBody = playerCollider.GetComponentInChildren<Rigidbody>();
        if( otherBody == null ) { return; }
        Vector3 force = (otherBody.velocity.normalized + Vector3.up * 0.5f) * KickForce;
        Vector3 ballToPlayer = (otherBody.transform.position - transform.position).normalized;
        Vector3 pos = transform.position + ballToPlayer * Radius;
        Body.AddForceAtPosition(force, pos, ForceMode.Impulse);
    }
}
