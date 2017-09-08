using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadShake : MonoBehaviour
{
    public class Uniforms
    {
        public static readonly int _Color = Shader.PropertyToID("_Color");
    }

    [SerializeField]
    [Tooltip("The camera  being tracked.")]
    Camera m_Camera;

    [SerializeField]
    [Tooltip("How fast the camera must move to initiate the nod gesture.")]
    float m_InitiateGestureThreashold = 3f;

    [SerializeField]
    [Tooltip("How much must the head move before a gesture is triggered.")]
    float m_MinimumShakeSize = 4f;

    [SerializeField]
    [Tooltip("How long may the gesture at most take.")]
    float m_MaxGestureRecordingTime = .1f;

    [SerializeField]
    [Tooltip("How many samples should be taken per second.")]
    float m_SamplesPerSecond = 100f;

    [SerializeField]
    [Tooltip("The target to change the color (temporary and will be removed).")]
    //GameObject m_Target;

    MeshRenderer m_Renderer;
	private CameraFollow CameraFollowScript;

    float m_PreviousAngle;

    float m_CameraDeltaPitch;

    bool m_GestureBeingRecorded;

    void Start()
    {
		CameraFollowScript = GetComponent<CameraFollow> ();
        //m_Renderer = m_Target.GetComponent<MeshRenderer>();

        m_PreviousAngle = GetCameraPitch();
        m_GestureBeingRecorded = false;
    }

    void FixedUpdate()
    {
        float cameraAngle = GetCameraPitch();

        // Delta pitch is how fast the camera is looking downwards.
        m_CameraDeltaPitch = cameraAngle - m_PreviousAngle;

        // If the camera looks down fast and no gesture is currently being detected
        // Do start the gesture recording
        if (m_CameraDeltaPitch > m_InitiateGestureThreashold && !m_GestureBeingRecorded)
        {
            m_GestureBeingRecorded = true;
            StartCoroutine(DoNodGestureRecord());
        }

        m_PreviousAngle = cameraAngle;
    }

    // Transform the [0; 360] angles into [-180; 180].
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
            // If in the editor, just grab the camera transform.
            angle = m_Camera.transform.eulerAngles.x;
        }
        else
        {
            // If in player, VR that is.
            angle = NormalizeAngle(UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye).eulerAngles.x);
        }

        // Normally one get 0 to 360 angular values.
        // however, it's more useful to get the -180 to 180 for gesture detection
        return NormalizeAngle(angle);
    }

    IEnumerator DoNodGestureRecord()
    {
        // How many samples to actually capture
        int numberOfSamples = (int)Mathf.Ceil(m_MaxGestureRecordingTime * m_SamplesPerSecond);

        // how long to wait in between sampling
        float waitTime = m_MaxGestureRecordingTime / numberOfSamples;

        // A value that gives the dominant direction of camera movement
        // Should be robust to quick vibrant movements
        float lowPassDelta = m_CameraDeltaPitch;

        float upperMeasure = GetCameraPitch();
        float lowerMeasure = upperMeasure;

        bool directionFlipped = false;

        // Record camera movement
        for (int i = 0; i < numberOfSamples; ++i)
        {
            // The current sample
            float sample = m_CameraDeltaPitch;

            float pitch = GetCameraPitch();

            upperMeasure = Mathf.Max(upperMeasure, pitch);
            lowerMeasure = Mathf.Min(lowerMeasure, pitch);

            // Smooth out the previous movement with the current
            lowPassDelta = lowPassDelta * .33f + sample * .66f;

            // If the dominant movement changes from downwards to upwards
            // Note that the direction flipped
            if (lowPassDelta < 0f)
            {
                directionFlipped = true;
            }

            // If the direction has flipped and the head has moved enough
            // register shake
            if (directionFlipped && (upperMeasure - lowerMeasure) > m_MinimumShakeSize)
            {
                // Set the color indicating that the effect fired
                //m_Renderer.material.SetColor(Uniforms._Color, Color.blue);

				// drops the object dragged by the camerafollowScript
				CameraFollowScript.m_Follow = false;
				CameraFollowScript.StartCoroutine ("CoolDown");
                // Let the color remain for a while
                yield return new WaitForSeconds(2f);
                break;
            }

            // If no nod was detected, wait for the next sample
            yield return new WaitForSeconds(waitTime);
        }

        //m_Renderer.material.SetColor(Uniforms._Color, Color.green);

        // 'Unlock' the gesture recording and return
        m_GestureBeingRecorded = false;
        yield return null;
    }
}
