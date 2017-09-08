using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivationTrigger : MonoBehaviour {

	private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
	public GameObject gameObject;

	void OnEnable()
	{
		m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
		m_EyeSelect.OnSelected += DisplayObject;
	}

	void DisplayObject(){
		if (!gameObject.activeSelf)
			gameObject.SetActive (true);
	}
}