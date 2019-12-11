Shader "Custom/RotateImage"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" { }
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _Speed ("Speed", float) = 2
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
        
        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
            
            //#pragma multi_compile DUMMY PIXELSNAP_ON
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float4 color: COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                fixed4 color: COLOR;
                float4 worldPosition: TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            float _Speed;
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            
            v2f vert(appdata v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                
                OUT.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                OUT.color = v.color * _Color;
                return OUT;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                float2 tempUV = i.uv;
                tempUV -= float2(0.5f, 0.5f);
                
                if (length(tempUV) > 0.5)
                {
                    return fixed4(0, 0, 0, 0);
                }
                
                float2 finalUV = 0;
                
                float angle = _Time.x * _Speed * 2;
                
                finalUV.x = tempUV.x * cos(angle) - tempUV.y * sin(angle);
                finalUV.y = tempUV.x * sin(angle) + tempUV.y * cos(angle);
                finalUV += float2(0.5f, 0.5f);
                fixed4 color = (tex2D(_MainTex, finalUV) + _TextureSampleAdd) * i.color;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                
                // #ifdef UNITY_UI_CLIP_RECT
                //     color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                // #endif
                
                #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                #endif
                
                return color;
            }
            ENDCG
            
        }
    }
}
