// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MSV Standard" {

	Properties {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0.01, 4)) = 0.25
		_AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.3

		_SrcBlend ("_SrcBlend", Float) = 1
		_DstBlend ("_DstBlend", Float) = 0

		[HideInInspector] _SrcBlend ("_SrcBlend", Float) = 1
		[HideInInspector] _DstBlend ("_DstBlend", Float) = 0
		[HideInInspector] _ZWrite ("_ZWrite", Float) = 1
	}

	SubShader {

		Pass {
			Tags {
				"LightMode" = "ForwardBase"
			}
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]			

			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile _ SHADOWS_SCREEN
			#pragma multi_compile _ VERTEXLIGHT_ON
			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
			#define FORWARD_BASE_PASS
			
			#pragma vertex vert
			#pragma fragment frag

			#include "MSV_Lighting.cginc"

			ENDCG
		}
		Pass {
			Tags {
				"LightMode" = "ForwardAdd"
			}
			Blend [_SrcBlend] One
			ZWrite Off
			
			CGPROGRAM
			#pragma target 3.0
			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
			#pragma multi_compile_fwdadd_fullshadows

			#pragma vertex vert
			#pragma fragment frag
			
			#include "MSV_Lighting.cginc"
			ENDCG
		}
		Pass {
			Tags {
				"LightMode" = "ShadowCaster"
			}
			Cull Off
			CGPROGRAM
			#pragma target 3.0
			
			#pragma multi_compile_shadowcaster
			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
			#pragma shader_feature _SMOOTHNESS_ALBEDO

			#pragma vertex shadow_vert
			#pragma fragment shadow_frag

			#include "MSV_Shadows.cginc"
			ENDCG
		}
	}
	CustomEditor "MSVStandardShaderGUI"
}
