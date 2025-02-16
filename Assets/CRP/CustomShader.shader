Shader "Custom/CustomShader"
{
    Properties
    {
        _LeftTex ("Left Texture", 2D) = "white" {}
        _RightTex ("Right Texture", 2D) = "white" {}
        _BackgroundTex ("Background Texture", 2D) = "white" {}

        _FrontTexOffset ("Front Texture Offset", Vector) = (-0.25, -0.25, 0, 0)
        _FrontTexScale ("Front Texture Scale", Vector) = (1.5, 1.5, 0, 0)
        _FrontTexRegion ("Front Texture Region", Vector) = (0, 0, 1, 1)

        _BackgroundTexOffset ("Background Texture Offset", Vector) = (0, 0, 0, 0)
        _BackgroundTexScale ("Background Texture Scale", Vector) = (1, 1, 0, 0)
        _BackgroundTexRegion ("Background Texture Region", Vector) = (0, 0, 1, 1)

    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            // Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uvLeft : TEXCOORD0;
				float2 uvRight : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uvLeft : TEXCOORD0;
				float2 uvRight : TEXCOORD1;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _LeftTex;
            float4 _LeftTex_ST;
            sampler2D _RightTex;
            float4 _RightTex_ST;

            sampler2D _BackgroundTex;
            
            float4 _FrontTexOffset;
            float4 _FrontTexScale;
            float4 _FrontTexRegion;
            
            float4 _BackgroundTexOffset;
            float4 _BackgroundTexScale;
            float4 _BackgroundTexRegion;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvLeft = TRANSFORM_TEX(v.uvLeft, _LeftTex);
				o.uvRight = TRANSFORM_TEX(v.uvRight, _RightTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                fixed4 colFront = fixed4(0, 0, 0, 0);
               
                if (unity_StereoEyeIndex == 0) {
                    // Scale and offset UV coordinates for both front and background textures
                    float2 scaledUVFront = i.uvLeft * _FrontTexScale.xy + _FrontTexOffset.xy;
                    // Check if the UV coordinates are within the specified regions
                    bool inFrontRegion = (scaledUVFront.x >= _FrontTexRegion.x && scaledUVFront.x <= _FrontTexRegion.z && scaledUVFront.y >= _FrontTexRegion.y && scaledUVFront.y <= _FrontTexRegion.w);
                    if (inFrontRegion){
                        colFront = tex2D(_LeftTex, i.uvLeft);
                    }
                }
                else {
                    // Scale and offset UV coordinates for both front and background textures
                    float2 scaledUVFront = i.uvRight * _FrontTexScale.xy + _FrontTexOffset.xy;
                    // Check if the UV coordinates are within the specified regions
                    bool inFrontRegion = (scaledUVFront.x >= _FrontTexRegion.x && scaledUVFront.x <= _FrontTexRegion.z && scaledUVFront.y >= _FrontTexRegion.y && scaledUVFront.y <= _FrontTexRegion.w);
                    if (inFrontRegion){
                        colFront = tex2D(_RightTex, i.uvRight);
                    }
                }

                float2 scaledUVBackground = i.uvRight * _BackgroundTexScale.xy + _BackgroundTexOffset.xy;
                bool inBackgroundRegion = (scaledUVBackground.x >= _BackgroundTexRegion.x && scaledUVBackground.x <= _BackgroundTexRegion.z && scaledUVBackground.y >= _BackgroundTexRegion.y && scaledUVBackground.y <= _BackgroundTexRegion.w);

                fixed4 colBackground = inBackgroundRegion ? tex2D(_BackgroundTex, scaledUVBackground) : fixed4(0, 0, 0, 0);
                
                fixed4 finalColor = lerp(colBackground, colFront, colFront.a);

                return finalColor;
            }
            ENDCG
        }
    }
}
