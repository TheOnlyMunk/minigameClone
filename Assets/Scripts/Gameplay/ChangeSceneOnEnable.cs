using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnEnable : MonoBehaviour
{
    [SerializeField]
    string m_SceneName;

    [SerializeField]
    float m_Delay;

    void OnEnable()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(m_Delay);
        SceneManager.LoadScene(m_SceneName);
        yield return null;
    }
}
