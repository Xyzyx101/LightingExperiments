Shader "Custom/ShadowCasterSprite"
{
	Properties
	{
		_MainTex ("Mask Texture (A)", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Float) = 0.3
	}
	SubShader
	{
		Name "ShadowCaster"
		Tags{ "Queue" = "Transparent" "LightMode" = "ShadowCaster" }
		Cull Off
		Pass {
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE
 
			#include "UnityCG.cginc"
			#include "UnityStandardShadow.cginc"
 
			struct v2f
			{
				V2F_SHADOW_CASTER;
			};
			
			//sampler2D _MainTex;
			//float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				//o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}
 
			float4 frag(v2f i) : COLOR
			{
				//clip(1);
				//fixed4 col = tex2D(_MainTex, i.uv);
				//clip( 1.f - col.a);
				//return 1.f - col.a;

				//return UnityEncodeCubeShadowDepth((length(i.vec) + unity_LightShadowBias.x) * _LightPositionRange.w)
				SHADOW_CASTER_FRAGMENT(i);
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//return col;
			}
 
			ENDCG
		}
    }
    FallBack Off
 }
	



	/*
	#define TRANSFER_SHADOW_CASTER_NORMALOFFSET(o) TRANSFER_SHADOW_CASTER_NOPOS(o,o.pos)

	*/

	/*
	  #define SHADOW_CASTER_FRAGMENT(i) return UnityEncodeCubeShadowDepth ((length(i.vec) + unity_LightShadowBias.x) * _LightPositionRange.w);
	*/

	/*
	// Shadow caster pass helpers

float4 UnityEncodeCubeShadowDepth (float z)
{
    #ifdef UNITY_USE_RGBA_FOR_POINT_SHADOWS
    return EncodeFloatRGBA (min(z, 0.999));
    #else
    return z;
    #endif
}*/

/*
// Declare all data needed for shadow caster pass output (any shadow directions/depths/distances as needed),
// plus clip space position.
#define V2F_SHADOW_CASTER V2F_SHADOW_CASTER_NOPOS UNITY_POSITION(pos)

// Vertex shader part, with support for normal offset shadows. Requires
// position and normal to be present in the vertex input.
#define TRANSFER_SHADOW_CASTER_NORMALOFFSET(o) TRANSFER_SHADOW_CASTER_NOPOS(o,o.pos)

// Vertex shader part, legacy. No support for normal offset shadows - because
// that would require vertex normals, which might not be present in user-written shaders.
#define TRANSFER_SHADOW_CASTER(o) TRANSFER_SHADOW_CASTER_NOPOS_LEGACY(o,o.pos)


*/