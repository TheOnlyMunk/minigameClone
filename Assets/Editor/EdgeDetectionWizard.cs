using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EdgeDetectionWizard : ScriptableWizard
{
    class Uniforms
    {
        public static readonly int _Sensitivity = Shader.PropertyToID("_Sensitivity");
        public static readonly int _BlurDirection = Shader.PropertyToID("_BlurDirection");
    }

    Shader m_Shader;
    Shader shader
    {
        get
        {
            if (!m_Shader)
            {
                m_Shader = Shader.Find("Hidden/Liquid Edge Filter");
            }

            return m_Shader;
        }
    }

    Material m_Material;
    Material material
    {
        get
        {
            if (!m_Material)
            {
                m_Material = new Material(shader);
            }

            return m_Material;
        }
    }

    public Texture2D source;

    [Range(0f, 1f)]
    public float sensitivity = .5f;

    [MenuItem("Tools/Create liquify edge texture")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create edge texture", typeof(EdgeDetectionWizard), "Create!");
    }

    void OnWizardUpdate()
    {
        helpString = "Generates edge information for a texture";

        if (null == source)
        {
            errorString = "Select a source texture";
            isValid = false;
        }
        else
        {
            errorString = "";
            isValid = true;
        }
    }

    void OnWizardCreate()
    {
        var path = EditorUtility.SaveFilePanel("Save texture as PNG", "", source.name + "-edges.png", "png");
        Debug.Log(path);

        RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 24, RenderTextureFormat.ARGB32);
        RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 24, RenderTextureFormat.ARGB32);
        temporary.Create();
        temporary2.Create();

        Camera activeCamera = Camera.main;
 
        // Initialize and render
        activeCamera.targetTexture = temporary;
        activeCamera.Render();
        RenderTexture.active = temporary;

        Graphics.Blit(source, RenderTexture.active, material, 0);

        for (int i = 0; i < 32; ++i)
        {
            material.SetVector(Uniforms._BlurDirection, new Vector4(1f, 0f, 0f, 0f));
            Graphics.Blit(temporary, temporary2, material, 1);

            material.SetVector(Uniforms._BlurDirection, new Vector4(0f, 1f, 0f, 0f));
            Graphics.Blit(temporary2, temporary, material, 1);
        }
 
        Texture2D output = new Texture2D(source.width, source.height, TextureFormat.ARGB32, source.mipmapCount > 0);

        // Read pixels
        output.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, true);
 
        // Clean up
        activeCamera.targetTexture = null;
        RenderTexture.active = null;

        DestroyImmediate(temporary);
        DestroyImmediate(temporary2);
        DestroyImmediate(m_Material);

        byte[] bytes = output.EncodeToPNG();
        File.WriteAllBytes(path, bytes);

        AssetDatabase.Refresh();
    }

    protected override bool DrawWizardGUI()
    {
        source = (Texture2D) EditorGUILayout.ObjectField("Source texture", source, typeof (Texture2D), false); 
        sensitivity = EditorGUILayout.Slider ("Sensitivity", sensitivity, 0f, 1f);

        if (source)
        {
            Rect rect = GUILayoutUtility.GetRect(512, 512);

            material.SetFloat(Uniforms._Sensitivity, sensitivity);
            Graphics.DrawTexture(rect, source, material, 0);
        }

        return true;
    }
}
