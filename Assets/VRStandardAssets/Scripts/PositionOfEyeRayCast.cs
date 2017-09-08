using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class PositionOfEyeRayCast : MonoBehaviour {

    private VRStandardAssets.Utils.VREyeRaycaster VREyeRayCast;

    // Use this for initialization
    void Start () {
        VREyeRayCast = GameObject.Find("Camera").GetComponent<VREyeRaycaster>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
