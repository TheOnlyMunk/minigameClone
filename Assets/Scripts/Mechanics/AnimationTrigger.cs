using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(VRStandardAssets.Utils.EyeSelect))]
public class AnimationTrigger : MonoBehaviour {

    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    enum Activation { Looking, EnterArea, DropArea };
    [Header("Interactions")]
    [SerializeField]
    private Activation ActivateWithThis;
    [Tooltip("Choose the Gameobject for triggering the events (Required for EnterArea & DropArea)")]
    [SerializeField]
    private GameObject TriggerDetection;

    [Header("Triggering")]
    private AnimationClip clip1;
    private AnimationClip clip2;

    [Header("Looking")]
    [Tooltip("Activate animation on initial look at object")]
    [SerializeField]
    private bool InitialLook;
    [Tooltip("Activate animation once the object have been looked at for some time")]
    [SerializeField]
    private bool SelectedLook;

    void OnEnable()
	{
        /* Creating the connections to the required scrips for each waay of 
        * triggering objects either by looking at, or the box collider.*/
        switch (ActivateWithThis)
        {
            case Activation.Looking:
                m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
                if(InitialLook)     m_EyeSelect.OnSelection += PlayAnimation;
                if(SelectedLook)    m_EyeSelect.OnSelected += PlayAnimation;
                break;
            case Activation.EnterArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += PlayAnimation;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
            case Activation.DropArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += PlayAnimation;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
        }
	}


    private void OnDisable()
    {
        switch (ActivateWithThis)
        {
            case Activation.Looking:
                m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
                if (InitialLook) m_EyeSelect.OnSelection -= PlayAnimation;
                if (SelectedLook) m_EyeSelect.OnSelected -= PlayAnimation;
                break;
            case Activation.EnterArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection -= PlayAnimation;
                break;
            case Activation.DropArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection -= PlayAnimation;
                break;
        }
    }

    void PlayAnimation(){

        gameObject.GetComponent<Animation>().clip = clip1;
        gameObject.GetComponent<Animation>().Play();

	}

}
