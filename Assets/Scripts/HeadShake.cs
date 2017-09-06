using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadShake : MonoBehaviour
{
    float[] m_Angles;

    Camera m_Camera;

    [SerializeField]
    [Range(10, 100)]
    int m_Samples;

    [SerializeField]
    float m_VarianceThreashold;

    float m_CenterAngle;

    public GameObject target;

    Color color;

    int index;

    void Start()
    {
        m_Camera = Camera.main;
        m_Angles = new float[m_Samples];

        m_CenterAngle = GetCameraPitch();

        color = Color.blue;
    }

    void FixedUpdate()
    {
        index = (index + 1) % m_Samples;

        m_Angles[index] = GetCameraPitch();

        if (index == m_Samples - 1)
        {
            m_CenterAngle = GetCameraPitch();

            CheckMovement();
        }
    }

    void CheckMovement()
    {
        bool up = false;
        bool down = false;

        for (int i = 0; i < m_Samples; ++i)
        {
            if (m_Angles[i] < m_CenterAngle - m_VarianceThreashold && !up)
            {
                up = true;
            }
            else if (m_Angles[i] > m_CenterAngle + m_VarianceThreashold && !down)
            {
                down = true;
            }
        }

        if (up && down)
        {
            Debug.Log("Wooo shake it baby");
            if (color == Color.blue)
            {
                color = Color.red;
            }
            else
            {
                color = Color.blue;
            }

            target.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180)
            return angle - 360f;
        else
            return angle;
    }

    float GetCameraPitch()
    {
        float angle = 0f;

        if (Application.isEditor)
        {
            angle = m_Camera.transform.eulerAngles.x;
        }
        else
        {
            angle = NormalizeAngle(UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye).eulerAngles.x);
        }

        return Input.acceleration.y;
    }
}
