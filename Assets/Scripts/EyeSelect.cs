using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRStandardAssets.Utils
{
    [RequireComponent(typeof(VRStandardAssets.Utils.VRInteractiveItem))]
    public class EyeSelect : MonoBehaviour
    {
        public event Action OnSelected;
        public event Action OnSelection;

        public float TimeTillSelected = 4.0f;

        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;

        private bool m_GazeOver;
        private float m_Timer = 0.0f;

        Coroutine m_Selected;


        private void OnEnable()
        {
            m_InteractiveItem = this.GetComponent<VRInteractiveItem>();
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
        }

        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
        }

        private void HandleOut()
        {
            // The user is no longer looking at the bar.
            m_GazeOver = false;

            // If the coroutine has been started (and thus we have a reference to it) stop it.
            if (m_Selected != null)
                StopCoroutine(m_Selected);
        }

        private void HandleOver()
        {
            //// The user is now looking at the bar.
            m_GazeOver = true;

            // If the user is looking at the object start the Selected coroutine and store a reference to it.
            if (m_GazeOver)
                m_Selected = StartCoroutine(Selected());
        }

        private IEnumerator Selected()
        {
            m_Timer = 0.0f;

            // LookingAtObject
            if (OnSelection != null)
                OnSelection();

            while(m_Timer < TimeTillSelected)
            {
                m_Timer += Time.deltaTime;

                // Wait until next frame.
                yield return null;

                // If the user is still looking at the object, go on to the next iteration of the loop.
                if (m_GazeOver)
                    continue;

                // If the user is no longer looking at the bar, reset the timer and leave the function.
                m_Timer = 0.0f;
                yield break;
            }

            // The gameobject is selected do something
            if (OnSelected != null)
                OnSelected();
            
        }

    }
}