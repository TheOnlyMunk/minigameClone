using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform FollowMe;
    public Transform Parrent;
    
    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    Coroutine m_Following;

    private bool m_Follow;
    
    public float speed = 10.0f;

	public Transform pusher;
	public Vector3 start;
	public Vector3 end;


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
        // Create object at the position of the Object, and set it as a child of the Camera.
        FollowMe = Instantiate(FollowMe, this.transform.position, Quaternion.identity, Parrent);


        // Start a coroutine to follow the object 
        m_Following = StartCoroutine(Following());

    }
    
    private IEnumerator Following()
    {
        m_Follow = true;
        while (m_Follow)
        {
			
//            float step = speed * Time.deltaTime;
//			transform.position = Vector3.MoveTowards(transform.position, FollowMe.position, step);

			// Addforce stuff
			pusher = this.gameObject.transform;

			//void FixedUpdate() {
				if ( pusher.transform.position == end){
					pusher.GetComponent<Rigidbody>().velocity = Vector3.zero;
					pusher.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
					pusher.GetComponent<Rigidbody>().AddForce((pusher.transform - FollowMe.transform) * 10);
				}/*else if (pusher.transform.position == start){
					pusher.GetComponent<Rigidbody>().velocity = Vector3.zero;
					pusher.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
					pusher.GetComponent<Rigidbody>().AddForce((FollowMe - pusher.transform.position) * 10);
				}*/
			//}



            // Wait until next frame.
            yield return null;

            // If the object should still be following.
            if (m_Follow)
                continue;

            // The object stopped following
            Destroy(FollowMe);
            yield break;
        }
    }
}
