using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFSM : MSV_Component
{

    Animator Anim;
    Rigidbody RB;
    PlayerMovement Movement;
    FSM<MSV_Actor> StateMachine;
    FSM<MSV_Actor> FooBarStateMachine;

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
        StateMachine = new FSM<MSV_Actor>(GetParentActor());

        StateMachine.AddState("Idle", Idle_Enter, Idle_Update, Idle_Exit);
        StateMachine.AddState("Run", Run_Enter, Run_Update, Run_Exit);

        FooBarStateMachine = new FSM<MSV_Actor>();
        FooBarStateMachine.AddState("FooBar", null, FooBar, null);
        FooBarStateMachine.AddState("FooUp", FooUp_Enter, FooUp_Update, FooUp_Exit);
        FooBarStateMachine.AddState("BarUp", BarUp_Enter, BarUp_Update, BarUp_Exit);
        StateMachine.AddSubstate("FooBar", FooBarStateMachine, "FooBar");
    }

    private void AnimStart() {
        StateMachine.ChangeState("Idle");
        StartCoroutine(TimerTest());
    }

    private void AnimUpdate() {
        StateMachine.Update();
    }

    IEnumerator TimerTest() {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("*************************************************************************");
        StateMachine.ChangeState("Idle");
        Debug.Log("*************************************************************************");
        yield return null;
    }

    private void Idle_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Idle_Enter");
    }
    private void Idle_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Idle_Update " + parent.name);
        fsm.ChangeState("Run");
    }
    private void Idle_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Idle_Exit");
    }
    private void Run_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Run_Enter");
    }
    private void Run_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Run_Update " + parent.name);
        fsm.ChangeState("FooBar");
    }
    private void Run_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Run_Exit");
    }

    private void FooBar(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("FooBar " + parent.name);
        fsm.ChangeState("FooUp");
    }

    private void FooUp_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("FooUp_Enter");
    }
    private void FooUp_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("FooUp_Update " + parent.name);
        fsm.ChangeState("BarUp");
    }
    private void FooUp_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("FooUp_Exit");
    }

    private void BarUp_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("BarUp_Enter");
    }
    private void BarUp_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("BarUp_Update " + parent.name);
        fsm.ChangeState("FooUp");
    }
    private void BarUp_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("BarUp_Exit");
    }
}
