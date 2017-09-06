using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour {

	private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
	public Text poemText;


	void Start(){
		poemText.text = " ";
	}

	void OnEnable()
	{
		m_EyeSelect = this.GetComponent<VRStandardAssets.Utils.EyeSelect>();
		m_EyeSelect.OnSelection += DisplayText;
	}
		
	void OnDisable()
	{
		m_EyeSelect.OnSelection -= DisplayText;
	}

	void DisplayText(){
		poemText.text = "Roses are red, Violets are blue. Smell poo";
		StartCoroutine ("RemoveText");
	}

	IEnumerator RemoveText(){

		yield return new WaitForSeconds (2);
		poemText.text = " ";

	}


}
