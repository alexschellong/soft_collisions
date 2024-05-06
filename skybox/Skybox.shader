Shader "Biyo Studios/Sky Shader"
{
    Properties
    {
        _SkyColor ("Sky Color", Color) = (1,1,1,1)
        _SunColor ("Sun Color", Color) = (1,1,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        LOD 100

        Cull Off ZWrite Off 

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 posOS    : POSITION;
            };

            struct v2f
            {
                float4 posCS        : SV_POSITION;
                float3 viewDirWS    : TEXCOORD0;
            };

            v2f Vertex(Attributes IN)
            {
                v2f OUT = (v2f)0;
    
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.posOS.xyz);
    
                OUT.posCS = vertexInput.positionCS;
                OUT.viewDirWS = vertexInput.positionWS;

                return OUT;
            }

            float3 _SunDir;
            float4 _SkyColor;
            float4 _SunColor;

            float4 Fragment (v2f IN) : SV_TARGET
            {
                float3 viewDir = normalize(IN.viewDirWS);

                float3 col = lerp(_SkyColor.rgb, _SunColor.rgb, step(0.9998,dot(_SunDir, viewDir)));
                return float4(col, 1);
            }
            ENDHLSL
        }
    }
}