using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(VRStandardAssets.Utils.VRInteractiveItem))]
public class TextureLiquify : MonoBehaviour
{
    class Uniforms
    {
        public static readonly int _Intensity = Shader.PropertyToID("_Intensity");
    }

    Shader m_Shader;
    Shader shader
    {
        get
        {
            if (!m_Shader)
            {
                m_Shader = Shader.Find("Custom/Liquify");
            }

            return m_Shader;
        }
    }

    [SerializeField]
    [Range(0f, 1f)]
    float m_EffectMultiplier = .5f;

    [SerializeField]
    float m_EffectBuildupDuratation = 1f;

    [SerializeField]
    MeshRenderer[] m_Renderers;

    VRStandardAssets.Utils.VRInteractiveItem m_InteractiveItem;

    Material[] m_Materials;

    Material[] m_WarpMaterials;

    float m_EffectLerp;
    float m_EffectTarget;

    bool m_LoopIsRunning;

    bool m_IsActive;

    void OnEnable()
    {
        if (m_Renderers.Length == 0)
        {
            List<MeshRenderer> rendererList = new List<MeshRenderer>();
            MeshRenderer ownRenderer = GetComponent<MeshRenderer>();
            if (ownRenderer)
            {
                rendererList.Add(ownRenderer);
            }

            List<MeshRenderer> childList = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
            foreach (MeshRenderer renderer in childList)
            {
                rendererList.Add(renderer);
            }

            m_Renderers = rendererList.ToArray();
        }


        if (m_Renderers.Length > 0)
        {
            m_Materials = new Material[m_Renderers.Length];
        }

        for (int i = 0; i < m_Renderers.Length; ++i)
        {
            m_Materials[i] = m_Renderers[i].material;
        }

        m_EffectLerp = 0f;

        m_InteractiveItem = GetComponent<VRStandardAssets.Utils.VRInteractiveItem>();

        m_InteractiveItem.OnOver += StartLiquify;
        m_InteractiveItem.OnOut += StopLiquify;
    }

    void OnDisable()
    {
        m_InteractiveItem.OnOver -= StartLiquify;
        m_InteractiveItem.OnOut -= StopLiquify;
    }

    void StartLiquify()
    {
        m_IsActive = true;
        m_EffectTarget = 1f;

        if (!m_LoopIsRunning)
        {
            StartCoroutine(LiquifyLoop());
        }
    }

    void StopLiquify()
    {
        m_IsActive = false;
        m_EffectTarget = 0f;

        if (!m_LoopIsRunning)
        {
            StartCoroutine(LiquifyLoop());
        }
    }

    IEnumerator LiquifyLoop()
    {
        m_LoopIsRunning = true;

        // If the warp materials are not created, make them.
        if (m_WarpMaterials == null)
        {
            m_WarpMaterials = new Material[m_Materials.Length];

            for (int i = 0; i < m_Materials.Length; ++i)
            {
                // Copy original material and replace the shader
                m_WarpMaterials[i] = new Material(m_Materials[i]);
                m_WarpMaterials[i].shader = shader;
            }
        }

        // Replace the materisl with a copy, bu with a warp shader.
        for (int i = 0; i < m_Renderers.Length; ++i)
        {
            m_Renderers[i].material = m_WarpMaterials[i];
        }

        // Move the effect intensity closer to the target.
        while (true)
        {
            float lerpProgress = Time.deltaTime / m_EffectBuildupDuratation;
            if (!m_IsActive)
            {
                lerpProgress = -lerpProgress;
            }

            m_EffectLerp = Mathf.Clamp(m_EffectLerp + lerpProgress, 0f, m_EffectTarget);

            for (int i = 0; i < m_WarpMaterials.Length; ++i)
            {
                m_Renderers[i].material.SetFloat(Uniforms._Intensity, m_EffectLerp * m_EffectMultiplier);
            }

            if (Mathf.Abs(m_EffectTarget - m_EffectLerp) < 1e-2f)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        m_EffectLerp = m_EffectTarget;

        // If the effect is inactive, cleanup the warp materials.
        if (!m_IsActive)
        {
            if (m_WarpMaterials != null)
            {
                for (int i = 0; i < m_Materials.Length; ++i)
                {
                    Destroy(m_WarpMaterials[i]);
                    m_WarpMaterials[i] = null;
                }

                m_WarpMaterials = null;
            }

            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                m_Renderers[i].material = m_Materials[i];
            }
        }

        m_LoopIsRunning = false;
        yield return null;
    }
}
