using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
    [SerializeField] private AudioClip m_OnFilledClip;
    [SerializeField] private AudioSource m_Audio;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
        m_EyeSelect.OnSelection += StartColor;
        m_EyeSelect.OnSelected += NewColor;   
    }

    private void OnDisable()
    {
        m_EyeSelect.OnSelection -= StartColor;
        m_EyeSelect.OnSelected -= NewColor;
    }

    private void NewColor()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        m_Audio.clip = m_OnFilledClip;

        if (m_Audio)
        {
            m_Audio.Play();
        }
    }

    private void StartColor()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

}
