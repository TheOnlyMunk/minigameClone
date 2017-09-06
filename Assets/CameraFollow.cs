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
    public float overShootPercentage = 10.20f;

    private void OnEnable()
    {
        m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
        m_EyeSelect.OnSelected += Selected;
    }

    private void OnDisable()
    {
        m_EyeSelect.OnSelected -= Selected;
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


            float step = speed * Time.deltaTime;


            Vector3 overShootPos = FollowMe.position + (FollowMe.position - transform.position) * overShootPercentage;

            float deltaP = 0.001f;
            if ((transform.position - overShootPos).magnitude < deltaP)
            {
                //start tweening to destinationPos rather than overShootPos. Possibly just
                //overShootPos = FollowMe.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, overShootPos, step);

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
