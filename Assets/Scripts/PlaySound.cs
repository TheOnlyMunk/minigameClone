using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
    //Variable only used if the soundclip should be sent to the soundsource from the script
    //[SerializeField] private AudioClip m_OnFilledClip;

    //Variable to hold the soundsource
    [SerializeField] private AudioSource m_Audio;

    private void OnEnable()
    {
        m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
        m_EyeSelect.OnSelected += PlayAudio;
    }

    private void OnDisable()
    {
        m_EyeSelect.OnSelected -= PlayAudio;
    }

    private void PlayAudio()
    {

        //Used to send the sound clip from the script to the source
        //m_Audio.clip = m_OnFilledClip;

        //Starts the sound source
        m_Audio.Play();
    }
}
