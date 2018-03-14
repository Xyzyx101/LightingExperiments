using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MSV_Action
{
    Jump,
    Interact,
    Fire1,
    Fire2
}

public enum MSV_Axis
{
    Vertical,
    Horizontal
}

public class MSV_Input
{
    private MSV_Input() { }

    private struct ActionBinding : IComparable<ActionBinding>
    {
        public MSV_Action Action;
        public string Name;
        public int CompareTo(ActionBinding that) {
            if( this.Action.CompareTo(that.Action) == 0 ) {
                return this.Action.CompareTo(that.Action);
            }
            return this.Name.CompareTo(that.Name);
        }
    }
    private struct AxisBinding : IComparable<AxisBinding>
    {
        public MSV_Axis Axis;
        public string Name;
        public int CompareTo(AxisBinding that) {
            if( this.Axis.CompareTo(that.Axis) == 0 ) {
                return this.Axis.CompareTo(that.Axis);
            }
            return this.Name.CompareTo(that.Name);
        }
    }

    static private bool ActionDirty = true;
    static private bool AxisDirty = true;
    static private List<ActionBinding> ActionBindings = new List<ActionBinding>()
    {
        new ActionBinding() { Action = MSV_Action.Jump, Name = "Jump"},
        new ActionBinding() { Action = MSV_Action.Interact, Name = "Interact"},
        new ActionBinding() { Action = MSV_Action.Fire1, Name = "Fire1"},
        new ActionBinding() { Action = MSV_Action.Fire2, Name = "Fire2"},
    };
    static private List<AxisBinding> AxisBindings = new List<AxisBinding>()
    {
        new AxisBinding() { Axis = MSV_Axis.Vertical, Name = "Vertical"},
        new AxisBinding() { Axis = MSV_Axis.Horizontal, Name = "Horizontal"},
    };

    static public bool GetAction(MSV_Action a) {
        if( ActionDirty ) {
            ActionBindings.Sort();
        };
        var actionIdx = ActionBindings.FindIndex(search => a == search.Action);
        if( actionIdx == -1 ) { return false; }
        while( actionIdx < ActionBindings.Count && ActionBindings[actionIdx].Action == a ) {
            if( Input.GetButton(ActionBindings[actionIdx].Name) ) {
                return true;
            }
            ++actionIdx;
        }
        return false;
    }

    static public bool GetActionPressed(MSV_Action a) {
        if( ActionDirty ) {
            ActionBindings.Sort();
        };
        var actionIdx = ActionBindings.FindIndex(search => a == search.Action);
        if( actionIdx == -1 ) { return false; }
        while( actionIdx < ActionBindings.Count && ActionBindings[actionIdx].Action == a ) {
            if( Input.GetButtonDown(ActionBindings[actionIdx].Name) ) {
                return true;
            }
            ++actionIdx;
        }
        return false;
    }

    static public bool GetActionReleased(MSV_Action a) {
        if( ActionDirty ) {
            ActionBindings.Sort();
        };
        var actionIdx = ActionBindings.FindIndex(search => a == search.Action);
        if( actionIdx == -1 ) { return false; }
        while( actionIdx < ActionBindings.Count && ActionBindings[actionIdx].Action == a ) {
            if( Input.GetButtonUp(ActionBindings[actionIdx].Name) ) {
                return true;
            }
            ++actionIdx;
        }
        return false;
    }

    static public float GetAxis(MSV_Axis a) {
        if( AxisDirty ) {
            AxisBindings.Sort();
        };
        var axisIdx = AxisBindings.FindIndex(search => a == search.Axis);
        if( axisIdx == -1 ) { return 0.0f; }
        float value = 0.0f;
        while( axisIdx < AxisBindings.Count && AxisBindings[axisIdx].Axis == a ) {
            var newValue = Input.GetAxis(AxisBindings[axisIdx].Name);
            if( Mathf.Abs(newValue) > Mathf.Abs(value) ) {
                value = newValue;
            }
            ++axisIdx;
        }
        return value;
    }

    static public void AddActionBinding(MSV_Action a, string name) {
        ActionBindings.Add(new ActionBinding() { Action = a, Name = name });
        ActionDirty = true;
    }

    static public void RemoveActionBinding(MSV_Action a, string name) {
        ActionBindings.RemoveAll(search => a == search.Action && name == search.Name);
        ActionDirty = true;
    }

    static public void RemoveAllActionBindings() {
        ActionBindings.Clear();
    }

    static public void AddAxisBinding(MSV_Axis a, string name) {
        AxisBindings.Add(new AxisBinding() { Axis = a, Name = name });
        AxisDirty = true;
    }

    static public void RemoveAxisBinding(MSV_Axis a, string name) {
        AxisBindings.RemoveAll(search => a == search.Axis && name == search.Name);
    }

    static public void RemoveAllAxisBindings() {
        AxisBindings.Clear();
    }
}
