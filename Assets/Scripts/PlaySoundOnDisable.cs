using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySoundOnDisable : MonoBehaviour
{
    [SerializeField]
    AudioClip m_AudioClip;
	[SerializeField]
	AudioMixerGroup mixer;

    void OnDisable()
    {
        GameObject soundObject = new GameObject();
        AudioSource audio = soundObject.AddComponent<AudioSource>();
        audio.clip = m_AudioClip;
		audio.outputAudioMixerGroup = mixer;
        audio.playOnAwake = true;
        audio.Play();
    }
}
