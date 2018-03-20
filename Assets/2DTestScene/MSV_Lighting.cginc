#if !defined(MSV_LIGHTING_INCLUDED)
#define MSV_LIGHTING_INCLUDED

#include "UnityPBSLighting.cginc"
#include "UnityCG.cginc"
#include "UnityStandardBRDF.cginc"
#include "AutoLight.cginc"

float4 _Tint;
sampler2D _MainTex;
float4 _MainTex_ST;
float4 _MainTex_TexelSize;
sampler2D _NormalMap;
float4 _NormalMap_ST;
float _Smoothness;
float _AlphaCutoff;
float _BumpScale;

sampler3D _DitherMaskLOD;

struct VertexData {
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
};

struct Interpolators {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float4 tangent : TEXCOORD2;
	float3 worldPos : TEXCOORD3;
	#if defined(VERTEXLIGHT_ON)
		float3 vertexLightColor : TEXCOORD4;
	#endif
	SHADOW_COORDS(5)
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
	i.pos = UnityObjectToClipPos(v.vertex);
	i.worldPos = mul(unity_ObjectToWorld, v.vertex);
	i.uv = TRANSFORM_TEX(v.uv, _MainTex);
	i.normal = UnityObjectToWorldNormal(v.normal);
	i.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
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
	//light.ndotl = DotClamped(i.normal, light.dir);
	light.ndotl = dot(i.normal, light.dir) * 0.5 + 0.5;
	return light;
}

float DitherPoint(float3 worldPos, float intensity) {
	float3 ditherCoords;
	ditherCoords.xy = worldPos.xy;
	ditherCoords.z = intensity;
	return tex3D(_DitherMaskLOD, ditherCoords);
}

float4 frag (Interpolators i) : SV_TARGET {
	// Pixel filter
	float2 texel = i.uv*_MainTex_TexelSize.zw;
	float2 hfw = 0.5*fwidth(texel);
	float2 fl = floor(texel - 0.5) + 0.5;
	float2 uv = (fl + smoothstep(0.5 - hfw, 0.5 + hfw, texel - fl))*_MainTex_TexelSize.xy;

	float4 mainColor = tex2Dgrad(_MainTex, uv, ddx(i.uv), ddy(i.uv));
	float3 albedo = mainColor.rgb * _Tint.rgb;	
	float alpha = _Tint.a;
	#if !defined(_SMOOTHNESS_ALBEDO)
		// texture alpha is ignore if yu are using alpha channel for smoothness
		alpha *= tex2D(_MainTex, i.uv.xy).a;
	#endif
	
	#if defined(_RENDERING_CUTOUT)
		clip(alpha - _AlphaCutoff);
	#endif

	#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
		float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
	#else
		float3 lightDir = _WorldSpaceLightPos0.xyz;
	#endif

	// Normal
	float lightBehind = DotClamped(i.normal, lightDir);
	float3 tNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv), _BumpScale).xyz;// * 2.0 - 1.0;
	tNormal = tNormal.xzy;
	float3 wNormal = normalize(i.normal);
	float3 wTangent = normalize(i.tangent);
	float binormal = cross(wNormal, wTangent) * (i.tangent.w * unity_WorldTransformParams.w);
	i.normal = normalize(
		tNormal.x * i.tangent +
		tNormal.y * i.normal +
		tNormal.z * binormal
	);
	UnityLight light = CreateLight(i);
	
	// Specular
	//float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
	//float3 halfVector = normalize(light.dir + viewDir);
	//float3 specular = 0;//light.color * pow(DotClamped(halfVector, i.normal), _Smoothness * 100);
			
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

	//color.rgb = clamp(color.rgb, (float3)0, albedo * saturate(light.color));
	
	color.a = 1.f;
	#if defined(_RENDERING_FADE) || defined(_RENDERING_TRANSPARENT)
		color.a = alpha;
	#endif
	return color;
}
#endif