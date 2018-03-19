using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    [SerializeField]
    private float RotateSpeed = 2.0f;

	void Update () {
        var rot = transform.eulerAngles;
        rot.y += RotateSpeed * Time.deltaTime;
        transform.eulerAngles = rot;
	}
}
