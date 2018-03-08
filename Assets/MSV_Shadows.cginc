#if !defined(MSV_SHADOWS_INCLUDED)
#define MSV_SHADOWS_INCLUDED

#include "UnityCG.cginc"

struct VertexData {
	float4 position : POSITION;
	float3 normal : NORMAL;
};

#if defined(SHADOWS_CUBE)
struct Interpolators {
	float4 position : SV_POSITION;
	float3 lightVec : TEXCOORD0;
};
	
Interpolators shadow_vert (VertexData v) {
	Interpolators i;
	i.position = UnityObjectToClipPos(v.position);
	i.lightVec = mul(unity_ObjectToWorld, v.position).xyz - _LightPositionRange.xyz;
	return i;
}
	
float4 shadow_frag (Interpolators i) : SV_TARGET {
	float depth = length(i.lightVec) + unity_LightShadowBias.x;
	depth *= _LightPositionRange.w;
	return UnityEncodeCubeShadowDepth(depth * 100.f);
}

#else

float4 shadow_vert (VertexData v) : SV_POSITION {
	float4 position = UnityClipSpaceShadowCasterPos(v.position.xyz, v.normal);
	return UnityApplyLinearShadowBias(position);
}

half4 shadow_frag () : SV_TARGET {
	return 0;
}

#endif //defined(SHADOWS_CUBE)

#endif //MSV_SHADOWS_INCLUDED