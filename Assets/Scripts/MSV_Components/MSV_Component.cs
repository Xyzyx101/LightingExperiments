using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MSV_Component : MonoBehaviour, System.IComparable<MSV_Component>
{
    // priority is intended to give control over the order that components on the same actor are run.  Higher runs first.
    virtual public int Priority {
        get {
            return 0;
        }
    }

    public int CompareTo(MSV_Component other) {
        return this.Priority.CompareTo(other.Priority);
    }

    private MSV_Actor _ParentActor;
    //public MSV_Actor ParentActor {
    //    get {
    //        return _ParentActor;
    //    }
    //    set {
    //        _ParentActor = ParentActor;
    //    }
    //}
    public void SetParentActor(MSV_Actor actor) {
        _ParentActor = actor;
    }
    public MSV_Actor GetParentActor() {
        return _ParentActor;
    }



    private MSV_Actor _OtherParentActor;
    public void SetOtherParentActor(MSV_Actor actor) {
        _OtherParentActor = actor;
    }
    public MSV_Actor GetOtherParentActor() {
        return _OtherParentActor;
    }

    public delegate void MSV_Awake();
    public delegate void MSV_Start();
    public delegate void MSV_Update();

    virtual public MSV_Awake AwakeDelegate {
        get {
            return null;
        }
    }

    virtual public MSV_Start StartDelegate {
        get {
            return null;
        }
    }

    virtual public MSV_Update UpdateDelegate {
        get {
            return null;
        }
    }
}
