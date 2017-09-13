using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomSelectionSlider))]
public class LanguageSwap : MonoBehaviour
{
    CustomSelectionSlider m_SelectionSlider;

    [SerializeField]
    LanguageManager m_LanguageManager;

    [SerializeField]
    LanguageManager.Language m_Language;

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
        m_LanguageManager.SetLanguage(m_Language);
    }
}
