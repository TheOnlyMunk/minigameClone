using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    CustomSelectionSlider m_SelectionSlider;

    [SerializeField]
    string m_TargetScene;

    void OnEnable()
    {
        m_SelectionSlider = GetComponent<CustomSelectionSlider>();

        m_SelectionSlider.OnBarFilled += DoChangeScene;
    }

    void OnDisable()
    {
        m_SelectionSlider.OnBarFilled -= DoChangeScene;
    }

    void DoChangeScene()
    {
        SceneManager.LoadScene(m_TargetScene);
    }
}
