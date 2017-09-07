using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform FollowMe;
    public Transform Parrent;
    
    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    Coroutine m_Following;

	public bool m_Follow;
	private bool canDrag = true;
    
    public float speed = 30.0f;
	public float detachRange = 5.0f;

	public Transform pusher;

	public Vector3 newPos; 
	private Vector3 endPoint;

	private Rigidbody pullRigid;

	void Start(){
		// 
		pullRigid = this.gameObject.transform.GetComponent<Rigidbody> ();
		FollowMe = Instantiate (FollowMe, this.transform.position, Quaternion.identity);
		pusher = this.gameObject.transform;
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
		if (canDrag) {
			//FollowMe.transform.position = this.transform.position;
			//FollowMe.transform.parent = Parrent;
			endPoint =  pusher.transform.position;
			// Create object at the position of the Object, and set it as a child of the Camera.

			// Start a coroutine to follow the object 
			m_Following = StartCoroutine (Following ());
		}
    }
		
    
    private IEnumerator Following()
    {
        m_Follow = true;
        while (m_Follow)
        {
			
			if (Vector3.Distance (endPoint, pusher.transform.position) < 4) {
				newPos = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
				newPos = Vector3.Normalize (newPos);
				float angle = Vector3.Angle (newPos, Camera.main.transform.forward);
				angle = (angle * Mathf.PI) / 180;
				float hypLength = Mathf.Abs (10f / Mathf.Cos (angle));
				endPoint = Camera.main.transform.position + (Camera.main.transform.forward.normalized * hypLength);

				// Addforce stuff
				pullRigid.velocity = Vector3.zero;
				pullRigid.angularVelocity = Vector3.zero;
				pullRigid.AddForce ((endPoint - pusher.transform.position) * speed);
			}else {
				m_Follow = false;

			}
				
            // Wait until next frame.
            yield return null;

            // If the object should still be following.
            if (m_Follow)
                continue;

            // The object stopped following
			//Destroy(FollowMe.gameObject);
			//pusher.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			//pusher.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			//pusher.GetComponent<Rigidbody> ().Sleep();
			pusher.GetComponent<Rigidbody> ().useGravity = true;
			FollowMe.transform.parent = null;
            yield break;
        }
    }


	public IEnumerator CoolDown()
	{
		canDrag = false;
		yield return new WaitForSeconds (1);
		canDrag = true;
	}

}
