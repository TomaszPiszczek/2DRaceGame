using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject carToFollow;
    public float cameraDistance = -10;  // Distance from the car
    public float cameraFOV = 60;        // Field of view

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component not found!");
        }
        else
        {
            cam.fieldOfView = cameraFOV;
        }
    }

    void LateUpdate()
    {
        transform.position = carToFollow.transform.position + new Vector3(0, 0, cameraDistance);
    }

    void Update()
    {
        // Adjust the camera FOV based on input for testing purposes
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraFOV += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraFOV -= 1f;
        }

        // Clamp the FOV to a reasonable range
        cameraFOV = Mathf.Clamp(cameraFOV, 20f, 120f);

        cam.fieldOfView = cameraFOV;
    }
}
