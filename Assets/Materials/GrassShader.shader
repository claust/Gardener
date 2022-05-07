Shader "Unlit/GrassShader" {
    Properties {
        _ColorA ("ColorA", Color) = (0 , 0, 0, 1)
        _ColorB ("ColorB", Color) = (0 , 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _Strength ("Strength", Range(0, 1)) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorA;
            float4 _ColorB;
            float _Strength;

            float3 hash(float3 p)
            {
                p = float3(dot(p, float3(127.1, 311.7, 74.7)),
                         dot(p, float3(269.5, 183.3, 246.1)),
                         dot(p, float3(113.5, 271.9, 124.6)));

                return frac(sin(p) * 43758.5453123);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Strength;
                return lerp(_ColorA, _ColorB, col);
            }
            ENDCG
        }
    }
}