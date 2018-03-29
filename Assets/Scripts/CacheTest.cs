using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheTest : MonoBehaviour
{
    struct TestStruct
    {
        public Collider MyCollider;
        public Transform MyTransform;
        public Rigidbody MyRigidBody;
    }

    TestStruct Test;

    private void Awake() {
        ComponentCache = new Dictionary<Type, Component>(4);
    }

    void Start() {
        const int count = 25000;
        var stopWatch = new System.Diagnostics.Stopwatch();

        stopWatch.Start();
        for( int i = 0; i < count; ++i ) {
            var x = LookupComponents();
        }
        Debug.Log("LookupComponents:" + stopWatch.ElapsedMilliseconds.ToString());
        stopWatch.Reset();

        stopWatch.Start();
        for( int i = 0; i < count; ++i ) {
            var x = LookupCachedComponents();
        }
        Debug.Log("LookupCachedComponents:" + stopWatch.ElapsedMilliseconds.ToString());
        stopWatch.Reset();

        stopWatch.Start();
        for( int i = 0; i < count; ++i ) {
            var x = LookupArrayCachedComponents();
        }
        Debug.Log("LookupArrayCachedComponents:" + stopWatch.ElapsedMilliseconds.ToString());
        stopWatch.Reset();
    }

    private TestStruct LookupComponents() {
        Test.MyCollider = GetComponent<Collider>();
        Test.MyTransform = GetComponent<Transform>();
        Test.MyRigidBody = GetComponent<Rigidbody>();
        return Test;
    }

    private TestStruct LookupCachedComponents() {
        Test.MyCollider = GetCachedComponent<Collider>();
        Test.MyTransform = GetCachedComponent<Transform>();
        Test.MyRigidBody = GetCachedComponent<Rigidbody>();
        return Test;
    }

    private TestStruct LookupArrayCachedComponents() {
        Test.MyCollider = GetArrayCachedComponent<Collider>();
        Test.MyTransform = GetArrayCachedComponent<Transform>();
        Test.MyRigidBody = GetArrayCachedComponent<Rigidbody>();
        return Test;
    }

    Dictionary<Type, Component> ComponentCache;
    private T GetCachedComponent<T>() where T : Component {
        Component comp;
        if( ComponentCache.TryGetValue(typeof(T), out comp) ) {
            return comp as T;
        } else {
            comp = GetComponent<T>();
            ComponentCache.Add(typeof(T), comp);
            return comp as T;
        }
    }

    public struct Pair<T> 
        where T : new() {
        public Pair(System.Type key, T value) {
            this.Key = key;
            this.Value = value;
        }
        public System.Type Key { get; set; }
        public T Value { get; set; }
    };

    List<Pair<Component>> PairCache = new List<Pair<Component>>();
    private T GetArrayCachedComponent<T>() where T : Component {
        Component comp;
        int index = PairCache.FindIndex(x => x.Key == typeof(T));
        if( index == -1 ) {
            comp = GetComponent<T>();
            PairCache.Add(new Pair<Component>(typeof(T), comp));
            return comp as T;
        } else {
            return PairCache[index].Value as T;
        }
    }
}
