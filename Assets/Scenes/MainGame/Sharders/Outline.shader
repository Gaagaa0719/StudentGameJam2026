Shader "Custom/Outline"
{
    Properties
    {
        _OutlineWidth("Outline Width", Float) = 0.05
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Tags { "LightMode" = "SRPDefaultUnlit" }

            ZTest LEqual
            ZWrite Off
            Cull Front

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                float _OutlineWidth;
                half4 _OutlineColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 extrusionDir = IN.positionOS.xyz;
                float len = length(extrusionDir);

                if (len > 0.0001)
                {
                    extrusionDir /= len;
                }
                else
                {
                    extrusionDir = float3(0, 1, 0);
                }

                IN.positionOS.xyz += extrusionDir * _OutlineWidth;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}