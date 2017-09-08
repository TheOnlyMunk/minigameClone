using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] private GameObject cam;

	void Awake(){
		Instantiate(cam);
	}


	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
