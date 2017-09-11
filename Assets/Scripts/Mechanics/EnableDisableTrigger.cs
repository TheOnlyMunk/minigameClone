using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRStandardAssets.Utils.EyeSelect))]
public class EnableDisableTrigger : MonoBehaviour {

    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    enum Activation { Looking, EnterArea, DropArea };
    [Header("Interactions")]
    [SerializeField]
    private Activation ActivateWithThis;
    [Tooltip("Choose the Gameobject for triggering the events (Required for EnterArea & DropArea)")]
    [SerializeField]
    private GameObject TriggerDetection;
    [Header("Looking")]
    [Tooltip("Enabk on initial look at object")]
    [SerializeField]
    private bool InitialLook;
    [Tooltip("Activate sound once the object have been looked at for some time")]
    [SerializeField]
    private bool SelectedLook;

    [Header("Choose Object")]
    [SerializeField]
    private GameObject ObjectToEnableOrDisable;
    [SerializeField]
    private State state;
    [SerializeField] public enum State { Enable, Disable};

    private void OnEnable()
    {

        /* Creating the connections to the required scrips for each waay of 
        * triggering objects either by looking at, or the box collider.*/
        switch (ActivateWithThis)
        {
            case Activation.Looking:
                m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
                if(InitialLook)     m_EyeSelect.OnSelection += EnableDisable;
                if(SelectedLook)    m_EyeSelect.OnSelected += EnableDisable;
                break;
            case Activation.EnterArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += EnableDisable;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
            case Activation.DropArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += EnableDisable;
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
                if (InitialLook) m_EyeSelect.OnSelection -= EnableDisable;
                if (SelectedLook) m_EyeSelect.OnSelected -= EnableDisable;
                break;
            case Activation.EnterArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection -= EnableDisable;
                break;
            case Activation.DropArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection -= EnableDisable;
                break;
        }
    }

    private void EnableDisable()
    {
        switch (state)
        {
            case State.Disable:
                ObjectToEnableOrDisable.SetActive(false);
                break;

            case State.Enable:
                ObjectToEnableOrDisable.SetActive(true);
                break;
        }
    }

}

