using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T> where T : class
{
    public delegate void StateEvent(FSM<T>fsm, T dataObj);
    private class State
    {
        public string Name;
        public StateEvent Enter;
        public StateEvent Update;
        public StateEvent Exit;
    }
    private class SubState : State
    {
        public FSM<T> SubStateMachine;
        public string DefaultState;
        public SubState() {
            Enter = new StateEvent(SubstateEnter);
            Update = new StateEvent(SubstateUpdate);
            Exit = new StateEvent(SubstateExit);
        }
        private void SubstateEnter(FSM<T> fsm, T dataObj) {
            SubStateMachine.ChangeState(DefaultState);
        }
        private void SubstateUpdate(FSM<T> fsm, T dataObj) {
            SubStateMachine.Update();
        }
        private void SubstateExit(FSM<T> fsm, T dataObj) {
            if( SubStateMachine.CurrentState != null ) {
                SubStateMachine.CurrentState.Exit(fsm, dataObj);
                SubStateMachine.StateQueue.Clear();
                SubStateMachine.CurrentState = null;
            }
        }
    }
    private class StateNameComparer : IComparer<State>
    {
        public int Compare(State x, State y) {
            return x.Name.CompareTo(y.Name);
        }
    }

    T DataObj;
    List<State> AllStates;
    Queue<StateEvent> StateQueue;
    State CurrentState;
    State NextState;
    static State SearchDummy = new State();
    static StateNameComparer StaticStateNameComparer = new StateNameComparer();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataObj">DataObj is optional.  It will be passed through to every state function in the machine.</param>
    public FSM(T dataObj = null) {
        DataObj = dataObj;
        AllStates = new List<State>();
        StateQueue = new Queue<StateEvent>();
    }

    public void AddState(string name, StateEvent enter, StateEvent update = null, StateEvent exit = null) {
        State newState = new State() { Name = name, Enter = enter, Update = update, Exit = exit };
        int index = AllStates.BinarySearch(newState, StaticStateNameComparer);
        if( index < 0 ) {
            AllStates.Insert(~index, newState);
        } else {
            Debug.LogWarning("State " + name + " already exist and cannot be added.");
        }
    }

    public void AddSubstate(string name, FSM<T> subStateMachine, string defaultSubState) {
        subStateMachine.DataObj = DataObj;
        SubState newState = new SubState() { Name = name, SubStateMachine = subStateMachine, DefaultState = defaultSubState };
        int index = AllStates.BinarySearch(newState, StaticStateNameComparer);
        if( index < 0 ) {
            AllStates.Insert(~index, newState);
        } else {
            Debug.LogWarning("State " + name + " already exist and cannot be added.");
        }
    }

    public void Update() {
        Debug.Assert(NextState != null, "FSM nextState is null.  Always call change state in Start() to initialize state");
        if( CurrentState == null || NextState.Name != CurrentState.Name ) {
            QueueChangeEvents();
        }
        while( StateQueue.Count > 1 ) {
            StateQueue.Dequeue()(this, DataObj);
        }
        if( StateQueue.Count > 0 ) {
            StateQueue.Peek()(this, DataObj);
        }
    }

    public void ChangeState(string stateName) {
        if( StateQueue.Count > 0 ) {
            StateQueue.Dequeue();
        }
        SearchDummy.Name = stateName;
        int index = AllStates.BinarySearch(SearchDummy, StaticStateNameComparer);
        if( index >= 0 ) {
            NextState = AllStates[index];
        } else {
            Debug.LogError("State " + stateName + " does not exist");
        }
    }

    void QueueChangeEvents() {
        if( CurrentState != null && CurrentState.Exit != null ) {
            StateQueue.Enqueue(CurrentState.Exit);
        }
        if( NextState.Enter != null ) {
            StateQueue.Enqueue(NextState.Enter);
        }
        if( NextState.Update != null ) {
            StateQueue.Enqueue(NextState.Update);
        }
        CurrentState = NextState;
    }
}
