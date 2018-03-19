Shader "RetroAA/Standard" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutputStandard o){
			float2 texel = IN.uv_MainTex*_MainTex_TexelSize.zw;
			float2 hfw = 0.5*fwidth(texel);
			float2 fl = floor(texel - 0.5) + 0.5;
			float2 uv = (fl + smoothstep(0.5 - hfw, 0.5 + hfw, texel - fl))*_MainTex_TexelSize.xy;

			fixed4 c = _Color*tex2Dgrad(_MainTex, uv, ddx(IN.uv_MainTex), ddy(IN.uv_MainTex));
			o.Albedo = c.rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
