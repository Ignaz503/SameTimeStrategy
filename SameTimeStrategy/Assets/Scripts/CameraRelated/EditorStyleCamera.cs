using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorStyleCamera : MonoBehaviour {


    public float lookSpeedH = 2f;
    public float lookSpeedV = 2f;
    public float zoomSpeed = 2f;
    public float dragSpeed = 6f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxisRaw("Mouse X");
            pitch -= lookSpeedV * Input.GetAxisRaw("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        //drag camera around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * dragSpeed, 0);
        }

        //Zoom in and out with Mouse Wheel
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
    }
}

