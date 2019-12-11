Shader "Unlit/Flash"
{
    Properties
    {
        //主纹理
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" { }
	 	// 主颜色
        _Color ("Tint", Color) = (1,1,1,1)
        // _MainTexSize ("Main Texture Size", Vector) = (1, 1, 0, 0)
        //流光纹理
        _FlashTex ("Flash Texture", 2D) = "white" { }
        //遮罩纹理
        _MaskTex ("Mask Texture", 2D) = "white" { }
        //流光颜色
        _FlashColor ("Flash Color", Color) = (1, 1, 1, 1)
        //流光强度
        _FlashIntensity ("Flash Intensity", Range(0, 1)) = 1
        //流光区域缩放
        _FlashScale ("Flash Scale", Range(0.1, 1)) = 1
        //水平流动速度
        _FlashTimeX ("Flash Time X", Range(-5, 5)) = 2
        //垂直流动速度
        _FlashTimeY ("Flash Time Y", Range(-5, 5)) = 0
        // //主纹理凸起值
        _RaisedValue ("Raised Value", Range(-0.5, 0.5)) = 0
        //流光能见度
        _Visibility ("Visibility", Range(0, 1)) = 1
        // 延迟
        _DelayTime ("Delay Time", Float) = 3
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        // LOD 100
        
        // Blend SrcAlpha OneMinusSrcAlpha

		// Tags
        // {
        //     "Queue"="Transparent"
        //     "IgnoreProjector"="True"
        //     "RenderType"="Transparent"
        //     "PreviewType"="Plane"
        //     "CanUseSpriteAtlas"="True"
        // }

        // Stencil
        // {
        //     Ref [_Stencil]
        //     Comp [_StencilComp]
        //     Pass [_StencilOp]
        //     ReadMask [_StencilReadMask]
        //     WriteMask [_StencilWriteMask]
        // }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        // ColorMask [_ColorMask]
    
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            // #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex: POSITION;
				float4 color : COLOR;
                float2 uv: TEXCOORD0;
                // float2 uv2: TEXCOORD1;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                // float2 uv2: TEXCOORD1;
				float4 color : COLOR;
                // UNITY_FOG_COORDS(1)
                float4 vertex: SV_POSITION;
            };
            
            float4 _MainTex_ST;
            sampler2D _MainTex;
            fixed4 _Color;
            // float4 _MainTexSize;
            // float4 _FlashTex_ST;
            sampler2D _FlashTex;
            sampler2D _MaskTex;
            fixed4 _FlashColor;
            fixed _FlashIntensity;
            fixed _FlashScale;
            fixed _FlashTimeX;
            fixed _FlashTimeY;
            fixed _RaisedValue;
            fixed _Visibility;
            fixed _DelayTime;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // o.uv2 = TRANSFORM_TEX(v.uv2, _FlashTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
				o.color = v.color * _Color;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                //=====================计算流光贴图的uv=====================
                // 缩放流光区域
                float2 flashUV = i.uv * _FlashScale;
                // float2 flashUV = float2(i.uv2.x * i.uv.yx , i.uv2.y * (_MainTexSize.x / _MainTexSize.y));
                // float2 flashUV = i.uv2.xy * _MainTexSize.xy;
                // 不断改变uv的x轴，让他往x轴方向移动
                // 每轮总时间=延迟播放时间+流光播放时间
                float tempTime = fmod(_Time.y, _DelayTime + _FlashTimeX);
                if (tempTime >= _DelayTime)
                {
                    tempTime -= _DelayTime;
                    flashUV.x += -tempTime * 2 / _FlashTimeX;
                }
                flashUV.x += 1;
                
                // flashUV.y += _FlashTimeY;
                //不断改变uv的y轴，让他往y轴方向移动
                // float tempTime2 = fmod(_Time.y, _DelayTime + _FlashTimeY);
                // if (tempTime2 >= _DelayTime)
                // {
                    //     tempTime2 -= _DelayTime;
                    //     flashUV.y += - tempTime2 * 2 / _FlashTimeY;
                    // }
                    // flashUV.y += 0;
                    
                    
                    
				//=====================计算流光贴图的可见区域=====================
				//取流光贴图的alpha值
				fixed4 flash = tex2D(_FlashTex, flashUV);
				// //取遮罩贴图的alpha值
				fixed maskAlpha = tex2D(_MaskTex, i.uv).a;
				//最终在主纹理上的可见值（flashAlpha和maskAlpha任意为0则该位置不可见）
				fixed visible = flash.a * maskAlpha * _FlashIntensity * _Visibility;
				
				//=====================计算主纹理的uv=====================
				//被流光贴图覆盖的区域凸起（uv的y值增加）
				float2 mainUV = i.uv;
				mainUV.y += visible * _RaisedValue;
				
				//=====================最终输出=====================
				//主纹理 + 可见的流光
                fixed4 texColor = tex2D(_MainTex, mainUV);
				fixed4 col = texColor * i.color + visible * flash.rgba * texColor.a;
				
				// UNITY_APPLY_FOG(i.fogCoord, col);

				// #ifdef UNITY_UI_CLIP_RECT
                // col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                // #endif

                // #ifdef UNITY_UI_ALPHACLIP
                // clip (col.a - 0.001);
                // #endif
                    return col;
                }
                ENDCG
                
            }
        }
    }
