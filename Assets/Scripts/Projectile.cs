using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float InitialVelocity = 10.0f;
    [SerializeField]
    private float TimeToLive = 3.0f;

    private Rigidbody Body;
    private GameObject DisplayQuad;
    private bool Hit = false;

    private void Awake() {
        Body = GetComponent<Rigidbody>();
        DisplayQuad = transform.GetChild(0).gameObject;
        Hit = false;
    }

    void Start() {
        Body.velocity += transform.TransformDirection(Vector3.forward + Vector3.up * 0.35f) * InitialVelocity;
        DisplayQuad.transform.eulerAngles = new Vector3(0, 0, Random.value * 360.0f);
        Body.AddTorque(Random.onUnitSphere * 20.0f);
    }

    private void Update() {
        float zRot = DisplayQuad.transform.eulerAngles.z;
        DisplayQuad.transform.eulerAngles = new Vector3(0, 0, zRot);
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Hit!!");
        if( !Hit ) {
            Hit = true;
            Destroy(this.gameObject, TimeToLive);
        }
    }

    public void AddIitialVelocity(Vector3 initVel) {
        Body.velocity += initVel;
    }
}
