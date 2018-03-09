using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat : MonoBehaviour
{

    public GameObject LookAtObj;

    private List<Light> Lights;

    void Start()
    {
        var lights = GameObject.FindObjectsOfType<Light>();
        Lights = new List<Light>(lights);
    }

    void Update()
    {
        Vector3 target = GetPriorityLightPosition();
        target.y = transform.position.y;
        Vector3 dir = target - transform.position;
        float flip = dir.z < 0.0f ? -1.0f : 1.0f;
        Vector3 localScale = transform.localScale;
        localScale.x =  Mathf.Abs(localScale.x) * flip;
        transform.localScale = localScale;
        transform.LookAt(target, Vector3.up);
    }

    Vector3 GetPriorityLightPosition()
    {
        float maxPriority = 0;
        Vector3 lightPos= Vector3.zero;
        for(int i = 0; i < Lights.Count; ++i)
        {
            float priorityFactor = Lights[i].intensity / Vector3.SqrMagnitude(Lights[i].transform.position - transform.position);
            //if(Lights[i].type == LightType.Directional) { priorityFactor *= 5.0f; }
            if(priorityFactor > maxPriority)
            {
                maxPriority = priorityFactor;
                lightPos = Lights[i].transform.position;
            }
        }
        return lightPos;
    }
}
