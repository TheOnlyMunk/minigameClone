using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [SerializeField] private GameObject cam;

	// Use this for initialization
	void Start () {
        Instantiate(cam);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
