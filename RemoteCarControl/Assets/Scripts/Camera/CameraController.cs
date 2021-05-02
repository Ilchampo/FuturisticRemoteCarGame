using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookTarget;
    public Transform Target;
    public float cameraSpeed = 0.125f;
    public Vector3 cameraOffset;
    public Vector3 velocity = Vector3.one;

    private void LateUpdate()
    {
        SmoothCamera();
    }

    private void SmoothCamera()
    {
        Vector3 desiredPosition = Target.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, cameraSpeed);
        transform.position = smoothedPosition;
        
        transform.LookAt(Target.position);

    }
}
