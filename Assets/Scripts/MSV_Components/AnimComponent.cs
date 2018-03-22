using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimComponent : MSV_Component
{
    [SerializeField]
    private float MoveAnimScale = 1.0f;
    [SerializeField]
    private float UpAnimScale = 1.0f;

    Animator Anim;
    Rigidbody RB;
    PlayerMovement Movement;

    override public int Priority {
        get {
            return -800;
        }
    }

    override public MSV_Awake AwakeDelegate {
        get {
            return new MSV_Awake(AnimAwake);
        }
    }

    //override public MSV_Start StartDelegate {
    //    get {
    //        return new MSV_Start(AnimStart);
    //    }
    //}

    override public MSV_Update UpdateDelegate {
        get {
            return new MSV_Update(AnimUpdate);
        }
    }

    private void AnimAwake() {
        RB = GetParentActor().GetComponentInChildren<Rigidbody>();
        Debug.Assert(RB != null, "Rigid body not found.  AnimComponent will not work.");
        Anim = GetParentActor().GetComponentInChildren<Animator>();
        Debug.Assert(Anim != null, "Animator not found.  AnimComponent will not work.");
        Movement = GetParentActor().GetComponentInChildren<PlayerMovement>();
        Debug.Assert(Movement != null, "PlayerMovement not found.  AnimComponent will not work.");
    }

    //private void AnimStart() {
    //    throw new NotImplementedException();
    //}

    private void AnimUpdate() {
        var vel = RB.velocity;
        Anim.SetFloat(AnimParams.Speed, Mathf.Clamp01(vel.magnitude * MoveAnimScale));
        //Anim.SetFloat(AnimParams.ForwardSpeed, Mathf.Clamp(vel.z * MoveAnimScale, -1.0f, 1.0f));
        //Anim.SetFloat(AnimParams.SideSpeed, Mathf.Clamp(vel.x * MoveAnimScale, -1.0f, 1.0f));
        //Anim.SetFloat(AnimParams.UpSpeed, Mathf.Clamp(vel.y * UpAnimScale, -1.0f, 1.0f));

        Vector3 facing = Movement.GetFacing();
        float faceFloat = Mathf.Atan2(facing.x, facing.z) / Mathf.PI;
        Debug.Log(facing);
        Anim.SetFloat(AnimParams.Facing, faceFloat);
    }
}
