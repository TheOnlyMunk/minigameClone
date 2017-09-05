using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;
using System;

public class Shake : MonoBehaviour {

    public event Action<Shake> OnSelect;

    [SerializeField] private VRStandardAssets.Utils.VRCameraFade m_CameraFade;
    [SerializeField] private VRStandardAssets.Utils.SelectionRadial m_SelectionRadial;
    [SerializeField] private VRStandardAssets.Utils.VRInteractiveItem m_InteractiveItem;
    [SerializeField] private bool m_LookAtCamera = true;
    [SerializeField] private float m_FollowSpeed = 10f;
    [SerializeField] private Transform m_Object;
    [SerializeField] private Transform m_Camera;

    private float m_DistanceFromCamera;

    private bool m_GazeOver;


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete; 
    }

    private void HandleOver()
    {
        m_SelectionRadial.Show();
        m_GazeOver = true;
    }

    private void HandleOut()
    {
        m_SelectionRadial.Hide();

        m_GazeOver = false;
    }

    private void HandleSelectionComplete()
    {
        if (m_GazeOver)
            StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        if (OnSelect != null)
            OnSelect(this);

        yield return new WaitForSeconds(1f);
    }

}
