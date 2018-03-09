// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShadowCaster3"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Cull Off
		//ZWrite Off
		//Pass {
		//	Tags {
		//		"LightMode" = "ForwardBase"
		//	}

		//	…
		//}

		//Pass {
		//	Tags {
		//		"LightMode" = "ForwardAdd"
		//	}

		//	…
		//}

		Pass {
			Tags {
				"LightMode" = "ShadowCaster"
			}

			CGPROGRAM

			#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct VertexData {
				float4 position : POSITION;
				float3 normal : NORMAL;
			};

			float4 vert (VertexData v) : SV_POSITION {
				float4 position = UnityClipSpaceShadowCasterPos(v.position.xyz, v.normal);
				return UnityApplyLinearShadowBias(position);
			}

			half4 frag () : SV_TARGET {
				return 0;
			}

			ENDCG
		}
	}
}
