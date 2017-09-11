using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventTriggerDetection : MonoBehaviour {

    public event Action OnTriggerDetection;
    public Dictionary<GameObject, bool> Targets = new Dictionary<GameObject, bool>();

    private void OnCollisionEnter(Collision collision)
    {
        if(Targets.ContainsKey(collision.gameObject))
        {
            //Debug.Log("Collision with target");
            if (OnTriggerDetection != null)
                OnTriggerDetection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Targets.ContainsKey(other.gameObject))
        {
            //Debug.Log("Triggering with target");
            if (OnTriggerDetection != null)
                OnTriggerDetection();
        }
    }
}
