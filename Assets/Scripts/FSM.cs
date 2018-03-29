using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T> where T : class
{
    public delegate void StateEvent(FSM<T> fsm, T dataObj);
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
        public StateEvent SubEnter;
        public StateEvent SubUpdate;
        public StateEvent SubExit;
        public SubState() {
            Enter = new StateEvent(SubstateEnter);
            Update = new StateEvent(SubstateUpdate);
            Exit = new StateEvent(SubstateExit);

        }
        private void SubstateEnter(FSM<T> fsm, T dataObj) {
            if( SubEnter != null ) {
                SubEnter(fsm, dataObj);
            }
        }
        private void SubstateUpdate(FSM<T> fsm, T dataObj) {
            if( SubUpdate != null ) {
                SubUpdate(fsm, dataObj);
            }
            SubStateMachine.Update();
        }
        private void SubstateExit(FSM<T> fsm, T dataObj) {
            SubStateMachine.CurrentState = null;
            if( SubExit != null ) {
                SubExit(fsm, dataObj);
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
            if( NextState == null ) {
                NextState = newState;
            }
        } else {
            Debug.LogWarning("State " + name + " already exist and cannot be added.");
        }
    }

    public void AddSubstate(string name, FSM<T> subStateMachine, StateEvent enter, StateEvent update = null, StateEvent exit = null) {
        subStateMachine.DataObj = DataObj;
        SubState newState = new SubState() { Name = name, SubStateMachine = subStateMachine, SubEnter = enter, SubUpdate = update, SubExit = exit };
        int index = AllStates.BinarySearch(newState, StaticStateNameComparer);
        if( index < 0 ) {
            AllStates.Insert(~index, newState);
            if( NextState == null ) {
                NextState = newState;
            }
        } else {
            Debug.LogWarning("State " + name + " already exist and cannot be added.");
        }
    }

    public void Update() {
        Debug.Assert(NextState != null, "FSM nextState is null.  You need to call change state at least once to set the initial state.");
        if( CurrentState == null || NextState.Name != CurrentState.Name ) {
            QueueEvents();
        }
        while( StateQueue.Count > 1 ) {
            StateQueue.Dequeue()(this, DataObj);
        }
        if( StateQueue.Count > 0 ) {
            StateQueue.Peek()(this, DataObj);
        }
    }

    public void ChangeState(string stateName) {
        if( CurrentState != null && CurrentState.Name == stateName ) { return; }
        if( StateQueue.Count > 0 ) {
            StateQueue.Dequeue();
        }
        SearchDummy.Name = stateName;
        int index = AllStates.BinarySearch(SearchDummy, StaticStateNameComparer);
        if( index >= 0 ) {
            if( AllStates[index] == null ) {
                Debug.Break();
            }
            NextState = AllStates[index];
        } else {
            Debug.LogError("State " + stateName + " does not exist");
        }
    }

    public void ChangeSubstate(string stateName) {
        if( CurrentState is SubState ) {
            var currentSubstate = CurrentState as SubState;
            currentSubstate.SubStateMachine.ChangeState(stateName);
        } else {
            Debug.LogWarning("Current state is not a substate");
        }
    }

    public string GetCurrentState() {
        return CurrentState.Name;
    }

    void QueueEvents() {
        if( CurrentState is SubState ) {
            var subState = CurrentState as SubState;
            if( subState.SubStateMachine.CurrentState.Exit != null ) {
                StateQueue.Enqueue(subState.SubStateMachine.CurrentState.Exit);
            }
        }
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
