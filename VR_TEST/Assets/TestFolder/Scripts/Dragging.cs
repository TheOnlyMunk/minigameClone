using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour {

    [SerializeField]
    private bool m_LookAtCamera = true;
    [SerializeField]
    private float m_FollowSpeed = 10f;
    [SerializeField]
    private Transform m_Object;
    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    private bool m_RotateWithCamera;

    private float m_DistanceFromCamera;

    void Start()
    {
        m_DistanceFromCamera = Vector3.Distance(m_Object.position, m_Camera.position);
    }

    void FixedUpdate ()
    {
		if(m_LookAtCamera)
        {
            m_Object.rotation = Quaternion.LookRotation(m_Object.position - m_Camera.position);
        }

        if(m_RotateWithCamera)
        {
            Vector3 targetDirection = Vector3.ProjectOnPlane(m_Camera.forward, Vector3.up).normalized;

            Vector3 targetPosition = m_Camera.position + targetDirection * m_DistanceFromCamera;

            targetPosition = Vector3.Lerp(m_Object.position, targetPosition, m_FollowSpeed * Time.deltaTime);

            targetPosition.y = m_Object.position.y;

            m_Object.position = targetPosition;
        }


	}
}
