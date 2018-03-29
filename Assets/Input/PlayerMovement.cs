using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MSV_Component
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

    private Rigidbody RB;
    private CapsuleCollider CapCollider;

    private Vector3 Facing;
    private float _FloorDistance;

    override public int Priority {
        get {
            return 100;
        }
    }

    override public MSV_Awake AwakeDelegate {
        get {
            return new MSV_Awake(Move_Awake);
        }
    }

    override public MSV_Update UpdateDelegate {
        get {
            return new MSV_Update(Move_Update);
        }
    }

    public void Move_Awake() {
        Facing = Vector3.back;
        RB = GetParentActor().GetComponentInChildren<Rigidbody>();
        Debug.Assert(RB != null, "Rigid body not found.  PlayerMovement will not work.");
        CapCollider = GetParentActor().GetComponentInChildren<CapsuleCollider>();
        Debug.Assert(CapCollider != null, "Capsule Collider is not found.  PlayerMovement will break.");
    }

    public void Move_Update() {
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
            RB.AddForce(Acceleration * mag * accelDir, ForceMode.Acceleration);
            Facing = accelDir;
        } else {
            if( RB.velocity.sqrMagnitude > 0.1f ) {
                // Decelerate
                Vector3 horizontalVel = RB.velocity;
                horizontalVel.y = 0.0f;
                horizontalVel.Normalize();
                RB.AddForce(Deceleration * -horizontalVel, ForceMode.Acceleration);
            }
        }
        UpdateFloorDistance();

        if( MSV_Input.GetActionPressed(MSV_Action.Jump) && IsOnGround) {
            RB.AddForce(JumpVelocity * Vector3.up, ForceMode.VelocityChange);
        }

        // Limit Max Velocity
        if( RB.velocity.x * RB.velocity.x + RB.velocity.z * RB.velocity.z > MaxVelocity * MaxVelocity ) {
            var horizontalVel = new Vector2(RB.velocity.x, RB.velocity.z);
            horizontalVel = horizontalVel.normalized * MaxVelocity;
            RB.velocity = new Vector3(horizontalVel.x, RB.velocity.y, horizontalVel.y); // those two Ys are not a typo
        }
    }

    public Vector3 GetFacing() {
        return Facing;
    }

    public bool IsOnGround {
        get {
            return _FloorDistance < 0.05f;
        }
    }
    public float FloorDistance {
        get {
            return _FloorDistance;
        }
    }
    private void UpdateFloorDistance() {
        const float errorOffset = 0.2f;
        Vector3 startOffset = CapCollider.center + Vector3.down * 0.5f * CapCollider.height + Vector3.up * errorOffset;
        Vector3 origin = transform.position + startOffset;
        RaycastHit hit;
        const float maxDistance = 0.5f;
        if( Physics.Raycast(origin, Vector3.down, out hit, maxDistance) ) {
            _FloorDistance = hit.distance - errorOffset; 
        } else {
            _FloorDistance = float.PositiveInfinity;
        }
    }
}
