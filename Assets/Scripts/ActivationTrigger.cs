using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivationTrigger : MonoBehaviour {

	private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
	public float fadeInDuration = 1.0f;
	private bool isActivated = false;

	void OnEnable()
	{
		m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
		m_EyeSelect.OnSelection += DisplayObject;
		m_EyeSelect.OnExit += FadeoutObject;
	}

	void OnDisable()
	{
		m_EyeSelect.OnSelection -= DisplayObject;
		m_EyeSelect.OnExit -= FadeoutObject;

	}
			

	void DisplayObject(){
		print ("activate");
		if (!transform.GetChild (0).gameObject.activeSelf) {
			transform.GetChild (0).gameObject.SetActive (true);
			StartCoroutine(FadeToFullAlpha(fadeInDuration, transform.GetChild(0).GetComponent<MeshRenderer>().material));
		}
	}

	void FadeoutObject(){
		print ("Deactivate");
		if (transform.GetChild (0).gameObject.activeSelf) {
			transform.GetChild (0).gameObject.SetActive (false);
		}

	}

	public IEnumerator FadeToFullAlpha(float t, Material i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
		while (i.color.a < 1.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
			yield return null;
		}
	}



}