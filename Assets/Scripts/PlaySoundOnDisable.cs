using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnDisable : MonoBehaviour
{
    [SerializeField]
    AudioClip m_AudioClip;

    void OnDisable()
    {
        GameObject soundObject = new GameObject();
        AudioSource audio = soundObject.AddComponent<AudioSource>();
        audio.clip = m_AudioClip;
        audio.playOnAwake = true;
        audio.Play();
    }
}
