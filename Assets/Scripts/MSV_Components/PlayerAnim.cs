using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class PlayerAnim : MSV_Component
{
    const float OneOverPI = 1.0f / Mathf.PI;
    FSM<MSV_Actor> FSM;
    PlayerMovement Movement;
    MSV_Animator Anim;
    Rigidbody RB;

    SpriteRenderer SpriteRend;

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

    override public MSV_Start StartDelegate {
        get {
            return new MSV_Start(AnimStart);
        }
    }

    override public MSV_Update UpdateDelegate {
        get {
            return new MSV_Update(AnimUpdate);
        }
    }

    private void AnimAwake() {
        Debug.Log("AnimAwake");
        FSM = new FSM<MSV_Actor>(GetParentActor());
        Anim = GetParentActor().GetComponentInChildren<MSV_Animator>();
        Debug.Assert(Anim != null, "MSV_Animator not found.  Player Animation will not work.");
        RB = GetParentActor().GetComponentInChildren<Rigidbody>();
        Debug.Assert(RB != null, "Rigid body not found.  Player Animation will not work.");
        Movement = GetParentActor().GetComponentInChildren<PlayerMovement>();
        Debug.Assert(Movement != null, "PlayerMovement not found.  Player Animation will not work.");
        SpriteRend = GetParentActor().GetComponentInChildren<SpriteRenderer>();
        Debug.Assert(Movement != null, "SpriteRenderer not found.  Player Animation will not work.");

        SetupStateMachineStates();
    }

    private void AnimStart() {
        //Debug.Log("AnimStart");
    }

    private void AnimUpdate() {
        //Debug.Log("AnimUpdate");
        FSM.Update();
        //Anim.DebugUpdate();
    }

    private void SetupStateMachineStates() {
        var idleSubState = new FSM<MSV_Actor>();
        idleSubState.AddState("IdleForward", IdleForward_Enter);
        idleSubState.AddState("IdleBack", IdleBack_Enter);
        idleSubState.AddState("IdleLeft", IdleLeft_Enter, null, IdleLeft_Exit);
        idleSubState.AddState("IdleRight", IdleRight_Enter);
        FSM.AddSubstate("Idle", idleSubState, null, Idle_Update, null);

        var runSubState = new FSM<MSV_Actor>();
        runSubState.AddState("RunForward", RunForward_Enter);
        runSubState.AddState("RunBack", RunBack_Enter);
        runSubState.AddState("RunLeft", RunLeft_Enter, null, RunLeft_Exit);
        runSubState.AddState("RunRight", RunRight_Enter);
        FSM.AddSubstate("Run", runSubState, null, Run_Update, null);

        FSM.AddState("Jump", Jump_Enter, Jump_Update, Jump_Exit);

        //Initialize FSM state
        FSM.ChangeState("Idle");
    }

    private void Idle_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        if( !Movement.IsOnGround ) {
            FSM.ChangeState("Jump");
        } else if( RB.velocity.sqrMagnitude > 0.01f ) {
            FSM.ChangeState("Run");
        } else {
            Vector3 facing = Movement.GetFacing();
            float faceFloat = Mathf.Atan2(facing.x, facing.z) * OneOverPI;
            if( faceFloat > -0.2f && faceFloat < 0.2f ) {
                FSM.ChangeSubstate("IdleForward");
            } else if( faceFloat > 0.3f && faceFloat < 0.7f ) {
                FSM.ChangeSubstate("IdleRight");
            } else if( faceFloat < -0.3f && faceFloat > -0.7f ) {
                FSM.ChangeSubstate("IdleLeft");
            } else if( Mathf.Abs(faceFloat) > 0.8f ) {
                FSM.ChangeSubstate("IdleBack");
            }
        }
    }

    private void IdleForward_Enter(FSM<MSV_Actor> actor, MSV_Actor parent) {
        Anim.Play("idleForward");
    }
    private void IdleBack_Enter(FSM<MSV_Actor> actor, MSV_Actor parent) {
        Anim.Play("idleBack");
    }
    private void IdleLeft_Enter(FSM<MSV_Actor> actor, MSV_Actor parent) {
        SpriteRend.flipX = true;
        Anim.Play("idleSide");
    }
    private void IdleLeft_Exit(FSM<MSV_Actor> actor, MSV_Actor parent) {
        SpriteRend.flipX = false;
    }
    private void IdleRight_Enter(FSM<MSV_Actor> actor, MSV_Actor parent) {
        Anim.Play("idleSide");
    }

    private void Run_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        if( !Movement.IsOnGround ) {
            FSM.ChangeState("Jump");
        } else if( RB.velocity.sqrMagnitude < 0.01f ) {
            FSM.ChangeState("Idle");
        } else {
            Vector3 facing = Movement.GetFacing();
            float faceFloat = Mathf.Atan2(facing.x, facing.z) * OneOverPI;
            if( faceFloat > -0.2f && faceFloat < 0.2f ) {
                FSM.ChangeSubstate("RunForward");
            } else if( faceFloat > 0.3f && faceFloat < 0.7f ) {
                FSM.ChangeSubstate("RunRight");
            } else if( faceFloat < -0.3f && faceFloat > -0.7f ) {
                FSM.ChangeSubstate("RunLeft");
            } else if( Mathf.Abs(faceFloat) > 0.8f ) {
                FSM.ChangeSubstate("RunBack");
            }
        }
    }

    private void RunForward_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Anim.Play("runForward");
    }
    private void RunLeft_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        SpriteRend.flipX = true;
        Anim.Play("runSide");
    }
    private void RunLeft_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        SpriteRend.flipX = false;
    }
    private void RunRight_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Anim.Play("runSide");
    }
    private void RunBack_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Anim.Play("runBack");
    }

    private void Jump_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("enter jump");
        Anim.Play("sideJump");
        Anim.PauseAnim("sideJump");
    }
    private void Jump_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        float upFactor = -RB.velocity.y * 0.333f * 0.5f + 0.5f;
        //Debug.Break();
        Debug.Log(RB.velocity.y + " " + upFactor);
        if( Movement.IsOnGround ) {
            FSM.ChangeState("Idle");
        }
        Anim.SetTime01("sideJump", upFactor);
    }
    private void Jump_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("exit jump");
    }
}
