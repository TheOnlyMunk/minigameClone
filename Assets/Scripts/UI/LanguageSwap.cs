using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomSelectionSlider))]
public class LanguageSwap : MonoBehaviour
{
    CustomSelectionSlider m_SelectionSlider;

    [SerializeField]
    GameObject m_SwapSource;

    [SerializeField]
    GameObject m_SwapTarget;

    void OnEnable()
    {
        m_SelectionSlider = GetComponent<CustomSelectionSlider>();

        m_SelectionSlider.OnBarFilled += Swap;
    }

    void OnDisable()
    {
        m_SelectionSlider.OnBarFilled -= Swap;
    }

    void Swap()
    {
        m_SwapTarget.SetActive(true);
        m_SwapSource.SetActive(false);
    }
}
