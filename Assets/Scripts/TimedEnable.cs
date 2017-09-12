using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEnable : MonoBehaviour {

    [SerializeField] [Range(0, 10)] private float TimeBeforeFirstEnable;
    [SerializeField] private List<GameObject> EnableFirst = new List<GameObject>();
    [SerializeField] [Range(0, 10)] private float TimeBeforeSecondEnable;
    [SerializeField] private List<GameObject> EnableSecond = new List<GameObject>();

    private void OnEnable()
    {
        StartCoroutine(TemporaryDisable(TimeBeforeFirstEnable, TimeBeforeSecondEnable));
    }

    IEnumerator TemporaryDisable(float firstDelay, float secondDelay)
    {
        yield return new WaitForSeconds(firstDelay);

        // Enable theese
        foreach (GameObject e in EnableFirst)
        {
            e.SetActive(true);
        }

        yield return new WaitForSeconds(secondDelay);

        // Disable theese
        foreach (GameObject e in EnableSecond)
        {
            e.SetActive(true);
        }

        yield return null;
    }

}
