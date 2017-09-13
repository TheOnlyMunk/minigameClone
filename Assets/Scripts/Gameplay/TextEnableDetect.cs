using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnableDetect : MonoBehaviour
{
    [SerializeField]
    Manager m_Manager;

    void OnEnable()
    {
        m_Manager.IncrementTextEnableCounter();
    }
}
