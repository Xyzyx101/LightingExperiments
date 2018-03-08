#if !defined(MSV_LIGHTING_INCLUDED)
#define MSV_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"
#include "UnityCG.cginc"
#include "UnityStandardBRDF.cginc"
#include "AutoLight.cginc"

float4 _Tint;
sampler2D _MainTex;
float4 _MainTex_ST;
float _Smoothness;
float _AlphaCutoff;

struct VertexData {
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
};

struct Interpolators {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
	#if defined(VERTEXLIGHT_ON)
		float3 vertexLightColor : TEXCOORD3;
	#endif
	SHADOW_COORDS(4)
};

void ComputeVertexLightColor (VertexData v, inout Interpolators i) {
	#if defined(VERTEXLIGHT_ON)
	i.vertexLightColor = Shade4PointLights(
		unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
		unity_LightColor[0].rgb, unity_LightColor[1].rgb,
		unity_LightColor[2].rgb, unity_LightColor[3].rgb,
		unity_4LightAtten0, i.worldPos, i.normal
	);
	#endif
}

Interpolators vert (VertexData v) {
	Interpolators i;
	i.uv = TRANSFORM_TEX(v.uv, _MainTex);
	i.pos = UnityObjectToClipPos(v.vertex);
	i.worldPos = mul(unity_ObjectToWorld, v.vertex);
	i.normal = mul(unity_ObjectToWorld, float4(v.normal, 0));
	i.normal = normalize(i.normal);
	TRANSFER_SHADOW(i);
	ComputeVertexLightColor(v, i);
	return i;
}

UnityLight CreateLight (Interpolators i) {
	UnityLight light;	
	#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
		light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
	#else
		light.dir = _WorldSpaceLightPos0.xyz;
	#endif
	
	UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);
		
	attenuation = floor(attenuation * 20.0) / 20.0;
	light.color = _LightColor0.rgb * attenuation;
	light.ndotl = DotClamped(i.normal, light.dir);
	return light;
}

float GetAlpha (Interpolators i) {
	float alpha = _Tint.a;
	#if !defined(_SMOOTHNESS_ALBEDO)
		// texture alpha is ignore if yu are using alpha channel for smoothness
		alpha *= tex2D(_MainTex, i.uv.xy).a;
	#endif
	return alpha;
}

float4 frag (Interpolators i) : SV_TARGET {
	float alpha = GetAlpha(i);
	#if defined(_RENDERING_CUTOUT)
		clip(alpha - _AlphaCutoff);
	#endif
	UnityLight light = CreateLight(i);
	i.normal = normalize(i.normal);
	float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

	float3 halfVector = normalize(light.dir + viewDir);
	float3 specular = 0;//light.color * pow(DotClamped(halfVector, i.normal), _Smoothness * 100);
				
	float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
	#if defined(_RENDERING_TRANSPARENT)
		albedo *= alpha;
		// if specular get added back in specular highlight should lower alpha
		//alpha = 1 - oneMinusReflectivity + alpha * oneMinusReflectivity;
	#endif
	float4 color = (float4)0;
	#if defined(VERTEXLIGHT_ON)
		color.rgb = albedo * i.vertexLightColor;
	#else
		color.rgb = albedo * light.color * light.ndotl;
	#endif	
		
	#if defined(FORWARD_BASE_PASS)
		float3 ambient = ShadeSH9(float4(i.normal, 1));
		color.rgb = dot(color, color) > dot(ambient, ambient) ? color : ambient;// max(color, ambient);
	#endif
	color.a = 1.f;
	#if defined(_RENDERING_FADE) || defined(_RENDERING_TRANSPARENT)
		color.a = alpha;
	#endif
	return color;
}

#endif