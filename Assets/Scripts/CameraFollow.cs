using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    Coroutine m_Following;

	public static bool m_Follow;
	private static bool canSelect = true;
    
	public bool rotateTowardsPlayer = true;
	public float rotationSpeed = 10.0f;
	public float followSpeed = 10.0f;
	public float detachRange = 5.0f;
	public float distanceFromPlayer = 10f;
	public static float shakeHeadCooldown = 1f;
	public float maxHeight = 10f;
	private static bool inDropZone; 

	private bool canFollow = true;
	private Manager gameManager;


	[SerializeField] private Transform m_camera;
	public Transform followObject;
	private Rigidbody followRigid;

	private Vector3 endPoint;

	static public CameraFollow instance; 

	void Awake()
	{
		instance = this;
	}


	void Start(){
		m_camera = Camera.main.transform;
		// assign rigidbody and gameobject of the follow object
		followRigid = this.gameObject.transform.GetComponent<Rigidbody> ();
		followObject = this.gameObject.transform;
		gameManager = GameObject.FindObjectOfType (typeof(Manager)) as Manager;
	}
		

    private void OnEnable()
    {
        m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
		m_EyeSelect.OnSelection += Selected;
    }

    private void OnDisable()
    {
		m_EyeSelect.OnSelection -= Selected;
    }

    private void Selected()
    {
		// make sure the object cannot be selected right after a headshake and while it following
		if (gameManager.pickedUpObject != null) {
			canFollow = false;
		} else {
			canFollow = true;
		}

		if (canSelect && !m_Follow && canFollow) {
			// set gravity of follow object to false, so this won't be taken into account when following
			followRigid.useGravity = false;
			// set the initial endpoint as the objects position, so it is certain that it won't be dropped because of detachRange
			endPoint = followObject.transform.position;

			// Start a coroutine to make the object follow the camera movement
			m_Following = StartCoroutine (Following ());
		}
    }
		
		
    
    private IEnumerator Following()
    {
		
        m_Follow = true;
        while (m_Follow)
        {
			// If the distance between the follow object and the endpoint is greater than detach range, it will be dropped
			if (Vector3.Distance (endPoint, followObject.transform.position) < detachRange) {

				Vector3 camLookDirVec = m_camera.forward;

				// Calculate the position of the object
				Vector3 camDir = new Vector3 (camLookDirVec.x, 0, camLookDirVec.z);
				camDir = Vector3.Normalize (camDir);
				float angle = Vector3.Angle (camDir, camLookDirVec);
				//translate degrees to radians
				angle = (angle * Mathf.PI) / 180;
				float hypLength = Mathf.Abs (distanceFromPlayer / Mathf.Cos (angle));
				endPoint = m_camera.transform.position + (camLookDirVec.normalized * hypLength);

				// if the endpoint if bigger than maxheight, the y coordinate of endpoint is locked at maxheight
				if (endPoint.y > maxHeight) {
					endPoint = new Vector3 (endPoint.x, maxHeight, endPoint.z);
				}

				if (rotateTowardsPlayer) {
					float step = rotationSpeed * Time.deltaTime;
					transform.rotation = Quaternion.RotateTowards(transform.rotation, m_camera.transform.rotation, step);
				}

				// Addforce stuff
				followRigid.velocity = Vector3.zero;
				followRigid.angularVelocity = Vector3.zero;
				// Addforce in the direction of endpoint
				followRigid.AddForce ((endPoint - followObject.transform.position) * followSpeed);

			}else {
				//Deselect ();
				m_Follow = false;
				gameManager.pickedUpObject = null;
			}
				
            // Wait until next frame.
			yield return new WaitForFixedUpdate();

            // If the object should still be following.
            if (m_Follow)
                continue;

			// if it should not follow
			// use gravity to make the object fall down
			followRigid.useGravity = true;
            yield break;
        }
    }

	/*void OnTriggerEnter(Collider other) {
		if (other.tag == "Dropzone") {
			print ("in dropzone");
			inDropZone = true;
		}
	}
	void OnTriggerExit(Collider other) {
		if (other.tag == "Dropzone") {
			print ("outside dropzone");
			inDropZone = false;
		}
	}*/

	// Drops the object
	public static void Deselect(){
		//if (inDropZone) {
			instance.StartCoroutine ("CoolDown");
			CameraFollow.m_Follow = false;
		//}
	}
		

	// cooldown, so that a dropped object by headshake can't be picked up right away
	public IEnumerator CoolDown()
	{
		canSelect = false;
		yield return new WaitForSeconds (shakeHeadCooldown);
		canSelect = true;
	}

}
