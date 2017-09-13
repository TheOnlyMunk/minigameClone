using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDisable : MonoBehaviour {

    [SerializeField] [Range(0, 10)] private float TimeBeforeFirstDisable;
    [SerializeField] private List<GameObject> DisableFirst = new List<GameObject>();
    [SerializeField] [Range(0, 10)] private float TimeBeforeSecondDisable;
    [SerializeField] private List<GameObject> DisableSecond = new List<GameObject>();

    private void OnEnable()
    {
        StartCoroutine(TemporaryDisable(TimeBeforeFirstDisable, TimeBeforeSecondDisable));
    }

    IEnumerator TemporaryDisable(float firstDelay, float secondDelay)
    {
        yield return new WaitForSeconds(firstDelay);

        // Enable theese
        foreach (GameObject e in DisableFirst)
        {
            e.SetActive(false);
        }

        yield return new WaitForSeconds(secondDelay);

        // Disable theese
        foreach (GameObject e in DisableSecond)
        {
            e.SetActive(false);
        }

        yield return null;
    }

}