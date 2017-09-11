using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VRStandardAssets.Utils.EyeSelect))]
public class PlaySound : MonoBehaviour
{
    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;

    enum Activation { Looking, EnterArea, DropArea };
    [Header("Interactions")]
    [SerializeField]
    private Activation ActivateWithThis;
    [Tooltip("Choose the Gameobject for triggering the events (Required for EnterArea & DropArea)")]
    [SerializeField]
    private GameObject TriggerDetection;
    [Header("Looking")]
    [Tooltip("Activate sound on initial look at object")]
    [SerializeField]
    private bool InitialLook;
    [Tooltip("Activate sound once the object have been looked at for some time")]
    [SerializeField]
    private bool SelectedLook;

    //Variable to hold the soundsource
    private AudioSource m_Audio;

    private void OnEnable()
    {

        m_Audio = this.GetComponent<AudioSource>();

        /* Creating the connections to the required scrips for each waay of 
        * triggering objects either by looking at, or the box collider.*/
        switch (ActivateWithThis)
        {
            case Activation.Looking:
                m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
                if(InitialLook)     m_EyeSelect.OnSelection += PlayAudio;
                if(SelectedLook)    m_EyeSelect.OnSelected += PlayAudio;
                break;
            case Activation.EnterArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += PlayAudio;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
            case Activation.DropArea:
                TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection += PlayAudio;
                TriggerDetection.GetComponent<EventTriggerDetection>().Targets.Add(this.gameObject, true);
                break;
        }
    }

    private void OnDisable()
    {
        if (this.gameObject)
        {
            switch (ActivateWithThis)
            {
                case Activation.Looking:
                    m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
                    if (InitialLook) m_EyeSelect.OnSelection -= PlayAudio;
                    if (SelectedLook) m_EyeSelect.OnSelected -= PlayAudio;
                    break;
                case Activation.EnterArea:
                    TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection -= PlayAudio;
                    break;
                case Activation.DropArea:
                    TriggerDetection.GetComponent<EventTriggerDetection>().OnTriggerDetection -= PlayAudio;
                    break;
            }
        }
    }

    private void PlayAudio()
    {
        //Starts the sound source
        m_Audio.Play();
    }
}
