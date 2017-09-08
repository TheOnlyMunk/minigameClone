using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateWithMouse : MonoBehaviour {

    [Range(0.0f, 5.0f)]
    public float SpeedHorizontal = 2.0f;
    [Range(0.0f, 5.0f)]
    public float SpeedVertical = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        yaw += SpeedHorizontal * Input.GetAxis("Mouse X");
        pitch -= SpeedVertical * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

	}
}
