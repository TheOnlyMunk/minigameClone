using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    Coroutine m_Following;

	public bool m_Follow;
	private bool canSelect = true;
    
	public bool rotateTowardsPlayer = true;
	public float rotationSpeed = 10.0f;
	public float followSpeed = 10.0f;
	public float detachRange = 5.0f;
	public float distanceFromPlayer = 10f;
	public float shakeHeadCooldown = 1f;
	public float maxHeight = 10f;

	public Transform followObject;
	private Rigidbody followRigid;

	public Vector3 newPos; 
	private Vector3 endPoint;

	void Start(){
		// assign rigidbody and gameobject of the follow object
		followRigid = this.gameObject.transform.GetComponent<Rigidbody> ();
		followObject = this.gameObject.transform;
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
		if (canSelect && !m_Follow) {
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
				
				// Calculate the position of the object
				newPos = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
				newPos = Vector3.Normalize (newPos);
				float angle = Vector3.Angle (newPos, Camera.main.transform.forward);
				//translate degrees to radians
				angle = (angle * Mathf.PI) / 180;
				float hypLength = Mathf.Abs (distanceFromPlayer / Mathf.Cos (angle));
				endPoint = Camera.main.transform.position + (Camera.main.transform.forward.normalized * hypLength);

				// if the endpoint if bigger than maxheight, the y coordinate of endpoint is locked at maxheight
				if (endPoint.y > maxHeight) {
					endPoint = new Vector3 (endPoint.x, maxHeight, endPoint.z);
				}

				if (rotateTowardsPlayer) {
					float step = rotationSpeed * Time.deltaTime;
					transform.rotation = Quaternion.RotateTowards(transform.rotation, Camera.main.transform.rotation, step);
				}

				// Addforce stuff
				followRigid.velocity = Vector3.zero;
				followRigid.angularVelocity = Vector3.zero;
				// Addforce in the direction of endpoint
				followRigid.AddForce ((endPoint - followObject.transform.position) * followSpeed);

			}else {
				m_Follow = false;
			}
				
            // Wait until next frame.
            yield return null;

            // If the object should still be following.
            if (m_Follow)
                continue;

			// if it should not follow
			// use gravity to make the object fall down
			followRigid.useGravity = true;
            yield break;
        }
    }

	// cooldown, so that a dropped object by headshake can't be picked up right away
	public IEnumerator CoolDown()
	{
		canSelect = false;
		yield return new WaitForSeconds (shakeHeadCooldown);
		canSelect = true;
	}

}
