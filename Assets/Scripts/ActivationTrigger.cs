using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivationTrigger : MonoBehaviour {

	private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
	private Coroutine lastRoutine = null;

	public float fadeInDuration = 1.0f;
	public float waitBeforeDisapear = 1.0f; 

	void OnEnable()
	{
		m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
		m_EyeSelect.OnSelection += EnableObject;
		m_EyeSelect.OnExit += DisableObject;
	}

	void OnDisable()
	{
		m_EyeSelect.OnSelection -= EnableObject;
		m_EyeSelect.OnExit -= DisableObject;

	}
			

	void EnableObject(){
		if (lastRoutine != null) {
			StopCoroutine (lastRoutine);
		}
		// check if childs are active. If not, then they are activated
		if (!transform.GetChild (0).gameObject.activeSelf) {
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(true);
			}
		}
		// start coroutine to fade-in the child objects 
		StartCoroutine(FadeToFullAlpha(fadeInDuration, transform.GetChild(0).GetComponent<TextMesh>()));
	}

	void DisableObject(){
		// check if childs are active. If they are, then WaitAndDisapear is executed
		if (transform.GetChild (0).gameObject.activeSelf) {
			lastRoutine = StartCoroutine(WaitAndDisapear(fadeInDuration, waitBeforeDisapear, transform.GetChild(0).GetComponent<TextMesh>()));
		}
	}
		

	// fadein to full alpha for all children 
	public IEnumerator FadeToFullAlpha(float t, TextMesh i)
	{
		// find all textmeshes in the gameobject and its children
		Component[] textMeshes = GetComponentsInChildren<TextMesh>();

		// Find the current color of the first child - this will be used to set the initial color and check when the alpha is as wanted
		i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);

		// loop through textmeshes and set their color until alpha is 1
		while (i.color.a < 1.0f)
		{
			foreach (TextMesh j in textMeshes)
			{
				j.color = new Color(j.color.r, j.color.g, j.color.b, j.color.a + (Time.deltaTime / t));
			}
			yield return null;
		}
	}

	// wait and then fadeout. The child gameobject is disabled after fadeout
	public IEnumerator WaitAndDisapear(float t, float wait, TextMesh i)
	{
		yield return new WaitForSeconds (waitBeforeDisapear);

		// find all textmeshes in the gameobject and its children
		Component[] textMeshes = GetComponentsInChildren<TextMesh>();

		// Find the current color of the first child - this will be used to set the initial color and check when the alpha is as wanted
		i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);

		// loop through textmeshes and set their color intil alpha is 0
		while (i.color.a > 0.0f)
		{
			foreach (TextMesh j in textMeshes)
			{
				j.color = new Color(j.color.r, j.color.g, j.color.b, j.color.a - (Time.deltaTime / t));
			}

			yield return null;
		}
		// loop through children and disable them
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(false);
		}

	}

}