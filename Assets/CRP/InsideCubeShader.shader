Shader "Custom/InsideCubeShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BackgroundTex ("Background Texture", 2D) = "white" {}
        _Region ("UV Region", Vector) = (0.25, 0, 0.5, 1)
        _BackgroundRegion ("UV Region Background", Vector) = (0.25, 0, 0.5, 1)
        _Offset ("Texture Offset", Vector) = (0, 0, 0, 0) 
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _BackgroundTex;
            float4 _Region;
            float4 _BackgroundRegion;
            float4 _Offset;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Pass through UVs without transformation
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uvMin = float2(_Region.x, _Region.y);
                float2 uvMax = float2(_Region.z, _Region.w);

                float2 bgUvMin = float2(_BackgroundRegion.x, _BackgroundRegion.y);
                float2 bgUvMax = float2(_BackgroundRegion.z, _BackgroundRegion.w);

                float2 uvSize = uvMax - uvMin;
                float2 bgUvSize = bgUvMax - bgUvMin;

                float2 offset = _Offset.xy;

                // Adjust UVs to match the desired portion of the texture
                float2 adjustedUV = (i.uv - uvMin) * (1.0 / uvSize) + offset;
                float2 adjustedBgUV = (i.uv - bgUvMin) * (1.0 / bgUvSize) + offset;

                fixed4 col = fixed4(0, 0, 0, 0);
                fixed4 bgCol = fixed4(0, 0, 0, 0);
                if (adjustedUV.x >= 0 && adjustedUV.x <= 1 && adjustedUV.y >= 0 && adjustedUV.y <= 1)
                {
                    col = tex2D(_MainTex, adjustedUV);
                }
                if (adjustedBgUV.x >= 0 && adjustedBgUV.x <= 1 && adjustedBgUV.y >= 0 && adjustedBgUV.y <= 1)
                {
                    bgCol = tex2D(_BackgroundTex, adjustedBgUV);
                }

                fixed4 finalCol = lerp(bgCol, col, col.a);

                return finalCol;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
