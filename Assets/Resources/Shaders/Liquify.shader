// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Liquify" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Rotate ("Rotate", Range(0, 1)) = 0.0
        _Intensity ("Intensity", Range(0, 1)) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface FragmentProgram Standard vertex:VertexProgram fullforwardshadows
        #pragma multi_compile __ WARP_VERTEX_SPACE
        #pragma target 3.0
        #include "Noise.cginc"

        static const float pi = 3.14159265359;

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;

        struct Input
        {
            float2 uv_MainTex;
        };

        struct Varyings
        {
            fixed4 color : COLOR;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Rotate;
        float _Intensity;

        float3 LiquifyVertex(float3 vertex, float multiplier)
        {
            float3 noiseValue = 0.;
            noiseValue.x = snoise(vertex * 3. + _Time.x * 5.75);
            noiseValue.y = snoise(vertex * 3. + _Time.x * 1.21);
            noiseValue.z = snoise(vertex * 3. + _Time.x * 0.75);

            return vertex + noiseValue * multiplier;
        }

        float2 LiquifyUV(float2 coord, float multiplier)
        {
            float noiseValue = snoise(coord + _Time.x * 1.75) * .5 + .5;
            float angle = noiseValue * 2. * pi;
            float range = sin(noiseValue) * .2;

            float2 offset = float2(cos(angle), sin(angle)) * range;
            return coord + offset * multiplier * _Intensity;
        }

        void VertexProgram (inout appdata_full v, out Input o)
        {
        #if defined(WARP_VERTEX_SPACE)
            v.vertex.xyz = LiquifyVertex(v.vertex.xyz, _Intensity);
        #endif
            UNITY_INITIALIZE_OUTPUT(Input, o);
        }

        void FragmentProgram (Input IN, inout SurfaceOutputStandard o)
        {
        #if !defined(WARP_VERTEX_SPACE)
            float2 texcoord = IN.uv_MainTex;
            float2 coord = LiquifyUV(IN.uv_MainTex, 1.);

            fixed4 normalColor = tex2D(_MainTex, texcoord) * _Color;
            fixed4 warpedColor = tex2D(_MainTex, coord) * _Color;
            o.Albedo = lerp(0., warpedColor.rgb, warpedColor.a);
        #else
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
        #endif

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
