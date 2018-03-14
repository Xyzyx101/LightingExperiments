using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSkew : MonoBehaviour
{
    public float Test = 0.5f;

    private Camera Camera;

    private void Awake() {
        Camera = GetComponent<Camera>();
    }

    void Start() {
        UpdateCameraMatrix();
    }

    private void Update() {
        UpdateCameraMatrix();
    }

    void UpdateCameraMatrix() {
        float cameraAngle = transform.eulerAngles.x;
        float aspect = Camera.aspect;
        float size = Camera.orthographicSize;
        //float bounce = Mathf.Sin(Time.time) + 2.0f;
        //Camera.orthographicSize = bounce;
        float horiz = 1 / (Camera.orthographicSize * Camera.aspect);
        float vert = 2.0f * Mathf.Sin(cameraAngle * Mathf.Deg2Rad) / Camera.orthographicSize;
        Matrix4x4 mat = Camera.projectionMatrix;
        mat[0, 0] = horiz;
        mat[1, 1] = vert;
        Camera.projectionMatrix = mat;
    }
}

//function SetObliqueness(horizObl: float, vertObl: float)
//{
//    var mat: Matrix4x4 = camera.projectionMatrix;
//    mat[0, 2] = horizObl;
//    mat[1, 2] = vertObl;
//    camera.projectionMatrix = mat;
//}