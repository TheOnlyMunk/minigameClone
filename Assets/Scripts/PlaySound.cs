using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
    [SerializeField] private AudioClip m_OnFilledClip;
    [SerializeField] private AudioSource m_Audio;

    // Use this for initialization
    void Start () {
		
	}

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
        m_Audio.clip = m_OnFilledClip;
        m_Audio.Play();
    }
}
