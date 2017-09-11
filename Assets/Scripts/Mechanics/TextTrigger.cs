using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour {

	[SerializeField]
	private VRStandardAssets.Utils.EyeSelect m_EyeSelect;
	private Text canvasText;
	private bool isShowing = false;


	[TextArea(3,10)]
	public string poemText = "This text will appear in a text area that automatically expands";
	public float fadeInDuration = 1.0f;


	void Start(){
		canvasText = gameObject.transform.GetChild (0).transform.GetChild (0).GetComponent<Text>();
		canvasText.text = " ";
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
		if (!isShowing) {
			isShowing = true;
			StartCoroutine(FadeTextToFullAlpha(fadeInDuration, canvasText));
			canvasText.text = poemText;
			//StartCoroutine ("RemoveText");
		}
	}


	public IEnumerator FadeTextToFullAlpha(float t, Text i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
		while (i.color.a < 1.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
			yield return null;
		}
	}

	public IEnumerator FadeTextToZeroAlpha(float t, Text i)
	{
		i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
		while (i.color.a > 0.0f)
		{
			i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
			yield return null;
		}
	}



	IEnumerator RemoveText(){

		yield return new WaitForSeconds (2);
		canvasText.text = " ";
		isShowing = false;

	}


}
