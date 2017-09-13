using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] private GameObject cam;
	[SerializeField] public GameObject pickedUpObject;

    [SerializeField]
    int m_TextNumberTarget = 1;

    [SerializeField]
    GameObject m_ObjectToEnable;

    int m_EnabledTexts;

	void Awake(){
		Instantiate(cam);
	}


	// Use this for initialization
	void Start ()
    {
        m_EnabledTexts = 0;
    }

    public void IncrementTextEnableCounter()
    {
        ++m_EnabledTexts;

        if (m_EnabledTexts == m_TextNumberTarget)
        {
            DoStuff();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DoStuff()
    {
        Debug.Log("Stuff was done!");
        m_ObjectToEnable.SetActive(true);
    }
}
