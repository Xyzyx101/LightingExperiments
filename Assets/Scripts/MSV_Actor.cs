using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSV_Actor : MonoBehaviour
{
    List<MSV_Component> MSV_Components;

    void Awake() {
        MSV_Components = new List<MSV_Component>(GetComponentsInChildren<MSV_Component>());
        MSV_Components.Sort();
        for( int compIdx = 0; compIdx < MSV_Components.Count; ++compIdx ) {
            RegisterComponent(MSV_Components[compIdx]);
        }
        for( int compIdx = 0; compIdx < MSV_Components.Count; ++compIdx ) {
            if( MSV_Components[compIdx].AwakeDelegate != null ) {
                MSV_Components[compIdx].AwakeDelegate();
            }
        }
    }

    void Start() {
        for( int compIdx = 0; compIdx < MSV_Components.Count; ++compIdx ) {
            if( MSV_Components[compIdx].StartDelegate != null ) {
                MSV_Components[compIdx].StartDelegate();
            }
        }
    }

    void Update() {
        for( int compIdx = 0; compIdx < MSV_Components.Count; ++compIdx ) {
            if( MSV_Components[compIdx].UpdateDelegate != null ) {
                MSV_Components[compIdx].UpdateDelegate();
            }
        }
    }

    public MSV_Component GetMSVComponent(System.Type type) {
        return MSV_Components.Find(item => type == item.GetType());
    }

    public List<MSV_Component> GetMSVComponents(System.Type type) {
        return MSV_Components.FindAll(item => type == item.GetType());
    }

    public void RegisterComponent(MSV_Component component) {
        if( component ) {
            if( !MSV_Components.Contains(component) ) {
                MSV_Components.Add(component);
            }
            component.SetParentActor(this);
            //component.SetParentActor(this);
            //Debug.Log(component.ParentActor);
        }
    }

    public void UnregisterComponent(MSV_Component component) {
        if( MSV_Components.Remove(component) ) {
            component.SetParentActor(null);
        }
    }
}
