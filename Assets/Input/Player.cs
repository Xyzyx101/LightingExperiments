using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject Projectile;

    private PlayerMovement PlayerMovement;
    private Rigidbody Body;
    private CapsuleCollider Capsule;

    private void Awake() {
        Body = GetComponent<Rigidbody>();
        Debug.Assert(Body != null, "Player rigid body is missing");
        Capsule = GetComponent<CapsuleCollider>();
        Debug.Assert(Capsule != null, "Player capsule collider is missing");
        PlayerMovement = GetComponent<PlayerMovement>();
        Debug.Assert(PlayerMovement != null, "PlayerMovement script is missing");

        PlayerMovement.Init(Body);
    }

    void Update() {
        // Moving
        PlayerMovement.MSV_Update();
        // Shooting
        if( MSV_Input.GetActionPressed(MSV_Action.Fire1) ) {
            Vector3 facing = PlayerMovement.GetFacing();
            Vector3 spawnPos = transform.TransformPoint(Capsule.center + Capsule.height * 0.25f * Vector3.up) + facing * (Capsule.radius + 0.1f);
            Quaternion spawnDir = Quaternion.LookRotation(facing, Vector3.up);
            Instantiate<GameObject>(Projectile, spawnPos, spawnDir);
        }

        if( MSV_Input.GetActionPressed(MSV_Action.Fire2) ) {
            
        }
    }
}
