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
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #include "Noise.cginc"

        static const float pi = 3.14159265359;

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;

        sampler2D _Edges;

        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Rotate;
        float _Intensity;

        float2 Liquify(float2 coord, float multiplier)
        {
            float noiseValue = snoise(coord + _Time.x * 1.75) * .5 + .5;
            float angle = noiseValue * 2. * pi;
            float range = sin(noiseValue) * .2;

            float2 offset = float2(cos(angle), sin(angle)) * range;
            return coord + offset * multiplier * _Intensity;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 texcoord = IN.uv_MainTex;
            float2 coord = Liquify(IN.uv_MainTex, 1.);

            fixed4 normalColor = tex2D(_MainTex, texcoord) * _Color;
            fixed4 warpedColor = tex2D(_MainTex, coord) * _Color;
            o.Albedo = lerp(0., warpedColor.rgb, warpedColor.a);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
