using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(VRStandardAssets.Utils.EyeSelect))]

public class ChangeColor : MonoBehaviour {

    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
    
    enum Activation {Looking, EnterArea, DropArea};
    [Header("Interactions")]
    [SerializeField]    private Activation ActivateWithThis;
    [Tooltip("Choose the Gameobject for Entering")]
    [SerializeField]    private GameObject TriggerDetection;
    [Header("Change")]
    [Tooltip("Choose the Gameobject which you want to change the color of")]
    [SerializeField]    private GameObject ColorThis;

    private void OnEnable()
    {
        /* Creating the connections to the required scrips for each waay of 
         * triggering objects either by looking at, or the box collider.
         */

        switch (ActivateWithThis)
        {
            case Activation.Looking:
                m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
                m_EyeSelect.OnSelection += StartColor;
                m_EyeSelect.OnSelected += NewColor;
                break;
            case Activation.EnterArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += NewColor;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
            case Activation.DropArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += NewColor;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
        }
 
    }

    private void OnDisable()
    {
        m_EyeSelect.OnSelection -= StartColor;
        m_EyeSelect.OnSelected -= NewColor;
    }

    private void NewColor()
    {
        ColorThis.GetComponent<Renderer>().material.color = Color.green;
    }

    private void StartColor()
    {
        ColorThis.GetComponent<Renderer>().material.color = Color.yellow;
    }

}
