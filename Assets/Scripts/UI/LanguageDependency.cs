using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageDependency : MonoBehaviour
{
    [SerializeField]
    LanguageManager.Language m_Language;

    public LanguageManager.Language GetLangugae()
    {
        return m_Language;
    }
}
