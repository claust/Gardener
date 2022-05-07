Shader "Unlit/FirstShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0.3, 0.87, 1)
        _Scale ("Scale", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct mesh_data
            {
                float4 vertex : POSITION;
                float3 normals: NORMAL;
                float2 uv: TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normals: TEXCOORD0;
                float2 uv: TEXCOORD1;
            };

            float4 _Color;
            float _Scale;

            v2f vert(mesh_data v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normals = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv * _Scale;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(i.uv, 0, 1);
            }
            ENDCG
        }
    }
}