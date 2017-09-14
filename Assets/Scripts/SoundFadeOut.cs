using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFadeOut : MonoBehaviour {

	public float fadeTime = 5.0f;
	public float delay = 5.0f;


	// Use this for initialization
	void Start () {
		StartCoroutine(FadeOut(gameObject.GetComponent<AudioSource>(), fadeTime, delay));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
		public IEnumerator FadeOut (AudioSource audioSource, float FadeTime, float delay) {
			float startVolume = audioSource.volume;

			yield return new WaitForSeconds(delay);

			while (audioSource.volume > 0) {
				audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

				yield return null;
			}

			audioSource.Stop ();
			audioSource.volume = startVolume;
		}
}
