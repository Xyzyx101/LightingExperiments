using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFSM : MSV_Component
{

    Animator Anim;
    Rigidbody RB;
    PlayerMovement Movement;
    FSM<MSV_Actor> StateMachine;

    override public int Priority {
        get {
            return -800;
        }
    }

    override public MSV_Awake AwakeDelegate {
        get {
            return new MSV_Awake(TestAwake);
        }
    }


    override public MSV_Start StartDelegate {
        get {
            return new MSV_Start(TestStart);
        }
    }

    override public MSV_Update UpdateDelegate {
        get {
            return new MSV_Update(TestUpdate);
        }
    }

    private void TestAwake() {
        StateMachine = new FSM<MSV_Actor>(GetParentActor());

        StateMachine.AddState("Idle", Idle_Enter, Idle_Update, Idle_Exit);
        StateMachine.AddState("Run", Run_Enter, Run_Update, Run_Exit);

        var fooBarMachine = new FSM<MSV_Actor>();
        fooBarMachine.AddState("Foo", Foo_Enter, Foo_Update, Foo_Exit);
        fooBarMachine.AddState("Bar", Bar_Enter, Bar_Update, Bar_Exit);

        StateMachine.AddSubstate("FooBar", fooBarMachine, SubTest_Enter, SubTest_Update, SubTest_Exit);
    }

    private void TestStart() {
        StateMachine.ChangeState("Idle");
        StartCoroutine(TimerTest());
    }

    private void TestUpdate() {
        StateMachine.Update();
    }

    IEnumerator TimerTest() {
        while( true ) {
            yield return new WaitForSeconds(3.0f);
            Debug.Log("*************************************************************************");
            StateMachine.ChangeState("Idle");
            Debug.Log("*************************************************************************");
        }
        //yield return null;
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

    //private void FooBar(FSM<MSV_Actor> fsm, MSV_Actor parent) {
    //    Debug.Log("FooBar " + parent.name);
    //    fsm.ChangeState("FooUp");
    //}

    private void Foo_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Foo_Enter");
    }
    private void Foo_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Foo_Update " + parent.name);
        fsm.ChangeState("Bar");
    }
    private void Foo_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Foo_Exit");
    }

    private void Bar_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Bar_Enter");
    }
    private void Bar_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Bar_Update " + parent.name);
        fsm.ChangeState("Foo");
    }
    private void Bar_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("Bar_Exit");
    }

    private void SubTest_Enter(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("SubTest_Enter");
        fsm.ChangeSubstate("Foo");
    }
    private void SubTest_Update(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("SubTest_Update " + parent.name);
    }
    private void SubTest_Exit(FSM<MSV_Actor> fsm, MSV_Actor parent) {
        Debug.Log("SubTest_Exit");
    }

}
