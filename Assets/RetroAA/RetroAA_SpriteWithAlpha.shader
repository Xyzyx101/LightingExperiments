// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RetroAA/SpriteWithAlpha"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,0,0,0)
	}
	SubShader
	{
		Tags {
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		//LOD 100

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			float4 _Color;

			v2f vert(appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				float2 texel = i.uv*_MainTex_TexelSize.zw;
				float2 hfw = 0.5*fwidth(texel);
				float2 fl = floor(texel - 0.5) + 0.5;
				float2 uv = (fl + smoothstep(0.5 - hfw, 0.5 + hfw, texel - fl))*_MainTex_TexelSize.xy;

				fixed4 col = tex2Dgrad(_MainTex, uv, ddx(i.uv), ddy(i.uv)); //bc

				// No premultiplied alpha, so we have to do it ourselves:
				//col *= col.a;

				//return col * i.color;

				//col.rgb *= col.a;

				return col* i.color;//bc
			}
			ENDCG
		}
	}
}
