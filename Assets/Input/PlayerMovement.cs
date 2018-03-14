﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float DIR_SCALE = Mathf.PI / 4.0f;
    [SerializeField]
    private float Acceleration = 10.0f;

    [SerializeField]
    private float Deceleration = 2.0f;

    [SerializeField]
    private float MaxVelocity = 3.0f;

    [SerializeField]
    private float JumpVelocity = 4.0f;

    private Rigidbody Body;

    private Vector3 Facing;

    public void Init(Rigidbody body) {
        Body = body;
    }

    public void MSV_Update() {
        var x = MSV_Input.GetAxis(MSV_Axis.Horizontal);
        var y = MSV_Input.GetAxis(MSV_Axis.Vertical);
        // if either direction is more than 0.1
        if( x * x + y * y > 0.01f ) {
            //Accelerate
            float dir = Mathf.Atan2(y, x) / DIR_SCALE;
            dir = Mathf.Floor(dir + 0.5f);
            dir *= DIR_SCALE;
            Vector3 accelDir = Mathf.Cos(dir) * Vector3.right + Mathf.Sin(dir) * Vector3.forward;
            float mag = Mathf.Sqrt(x * x + y * y);
            Body.AddForce(Acceleration * mag * accelDir, ForceMode.Acceleration);
            Facing = accelDir;
        } else {
            if( Body.velocity.sqrMagnitude > 0.1f ) {
                // Decelerate
                Vector3 horizontalVel = Body.velocity;
                horizontalVel.y = 0.0f;
                horizontalVel.Normalize();
                Body.AddForce(Deceleration * -horizontalVel, ForceMode.Acceleration);
            }
        }

        // Limit Max Velocity
        if( Body.velocity.x * Body.velocity.x + Body.velocity.z * Body.velocity.z > MaxVelocity * MaxVelocity ) {
            var horizontalVel = new Vector2(Body.velocity.x, Body.velocity.z);
            horizontalVel = horizontalVel.normalized * MaxVelocity;
            Body.velocity = new Vector3(horizontalVel.x, Body.velocity.y, horizontalVel.y); // those two y are not a typo
        }

        if(MSV_Input.GetActionPressed(MSV_Action.Jump)) {
            Body.AddForce(JumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }
    }

    public Vector3 GetFacing() {
        return Facing;
    }
}