using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour {


	private VRStandardAssets.Utils.EyeSelect m_EyeSelect;


	void OnEnable()
	{
		m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
		m_EyeSelect.OnSelection += PlayAnimation;

	}


	void OnDisable()
	{
		m_EyeSelect.OnSelection -= PlayAnimation;
	}

	void PlayAnimation(){

		gameObject.GetComponent<Animation> ().Play();

	}

}
