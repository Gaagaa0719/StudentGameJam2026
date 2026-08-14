Shader "Custom/Metaball"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("Scale", Range(0, 0.05)) = 0.01
        _Cutoff ("Cutoff", Range(0, 0.5)) = 0.01
    }

    SubShader
    {
        // URP用のタグ
        Tags {
            // 半透明オブジェクトを書き込む工程でレンダリングする設定
            "Queue" = "Transparent"

            // 値は何でもいいらしい
            "RenderType" = "Transparent"

            // URPに対応していることの明示
            "RenderPipeline" = "UniversalRenderPipeline"
        }

        // (1 - このオブジェクトの透明度)をもともとこのピクセルに書き込んであった色に掛けて足す。
        Blend One OneMinusSrcAlpha

        // Zバッファへの書き込みをオフ
        ZWrite Off

        // 一切カリングを行わない
        Cull Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half _Scale;
                half _Cutoff;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv - 0.5;

                float a = 1.0 / (uv.x * uv.x + uv.y * uv.y);
                a *= _Scale;

                half4 color = IN.color * a;

                clip(color.a - _Cutoff);

                return color;
            }
            ENDHLSL
        }
    }
}
