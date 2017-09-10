Shader "Hidden/Liquid Edge Filter"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Off Blend Off

        CGINCLUDE
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;

        float _Sensitivity;
        float2 _BlurDirection;

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float2 uv : TEXCOORD0;
            float2 texcoord : TEXCOORD1;
            float4 vertex : SV_POSITION;
        };

        Varyings VertDefault (appdata v)
        {
            Varyings o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            o.texcoord = v.uv;
            return o;
        }

        float4 GaussianBlurFragment5tap(Varyings i) : SV_Target
        {
            float2 offset = _MainTex_TexelSize.xy * _BlurDirection;

            float4 blurred = 0.;

            blurred += tex2D(_MainTex, i.texcoord + offset * -4.) * .091638;
            blurred += tex2D(_MainTex, i.texcoord + offset *  4.) * .091638;

            blurred += tex2D(_MainTex, i.texcoord + offset * -3.) * .105358;
            blurred += tex2D(_MainTex, i.texcoord + offset *  3.) * .105358;

            blurred += tex2D(_MainTex, i.texcoord + offset * -2.) * .116402;
            blurred += tex2D(_MainTex, i.texcoord + offset *  2.) * .116402;

            blurred += tex2D(_MainTex, i.texcoord + offset * -1.) * .123572;
            blurred += tex2D(_MainTex, i.texcoord + offset *  1.) * .123572;

            float4 centerTap = tex2D(_MainTex, i.texcoord);
            blurred += centerTap * 0.126063;

            return max(blurred, centerTap);
        }

        float2 Gradient(float up, float down, float left, float right)
        {
            float horizontal = (right - left) / 2.;
            float vertical = (up - down) / 2.;

            return float2(horizontal, vertical);
        }

        fixed4 EdgeDetectionAlpha(Varyings i) : SV_Target
        {
            float4 offset = float4(1., 1., -1., 0.) * _MainTex_TexelSize.xyxy;

            float up = tex2D(_MainTex, i.uv + offset.wy).a;
            float down = tex2D(_MainTex, i.uv - offset.wy).a;
            float left = tex2D(_MainTex, i.uv - offset.xw).a;
            float right = tex2D(_MainTex, i.uv + offset.xw).a;

            float2 gradient = Gradient(up, down, left, right);

            float4 edge = step(_Sensitivity, length(gradient));
            edge.a = 1.;

            return edge;
        }
        ENDCG

        Pass // 0
        {
            CGPROGRAM
            #pragma vertex VertDefault
            #pragma fragment EdgeDetectionAlpha
            ENDCG
        }

        Pass // 1
        {
            CGPROGRAM
            #pragma vertex VertDefault
            #pragma fragment GaussianBlurFragment5tap
            ENDCG
        }
    }
}
