using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangugaeTextSwap : MonoBehaviour
{
    [SerializeField]
    LanguageManager.Language m_Language;

    [SerializeField]
    string m_Text;

    public LanguageManager.Language GetLangugae()
    {
        return m_Language;
    }

    public void DoSwap()
    {
        TextMesh textMesh = GetComponent<TextMesh>();
        textMesh.text = m_Text;
    }
}
