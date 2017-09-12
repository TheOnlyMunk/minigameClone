using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public enum Language
    {
        DK,
        EN
    }

    [SerializeField]
    Language m_Langugae;

    bool m_WasBrought = false;

    void OnAwake()
    {
        FilterLanguageObjects();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;

        DontDestroyOnLoad(this);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene load");
        m_WasBrought = true;

        FilterLanguageObjects();
    }

    void FilterLanguageObjects()
    {
        LanguageDependency[] languageObjects = Resources.FindObjectsOfTypeAll(typeof(LanguageDependency)) as LanguageDependency[];

        foreach (LanguageDependency language in languageObjects)
        {
            if (language.GetLangugae() == m_Langugae)
            {
                language.gameObject.SetActive(true);
            }
            else
            {
                language.gameObject.SetActive(false);
            }
        }
    }

    public void SetLanguage(Language language)
    {
        bool shouldRefresh = true;
        if (language == m_Langugae)
        {
            shouldRefresh = true;
        }

        m_Langugae = language;

        if (shouldRefresh)
        {
            FilterLanguageObjects();
        }
    }

    public Language GetCurrentLangugae()
    {
        return m_Langugae;
    }
}
