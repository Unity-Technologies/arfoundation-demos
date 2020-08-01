#ifndef SHADERGRAPH_PREVIEW
real SampleShadowmapDirect(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), SamplerState PointClamp, float4 shadowCoord, ShadowSamplingData samplingData, half4 shadowParams, bool isPerspectiveProjection = true)
{

	// Compiler will optimize this branch away as long as isPerspectiveProjection is known at compile time
	if (isPerspectiveProjection)
		shadowCoord.xyz /= shadowCoord.w;

	real distance;
	real shadowStrength = shadowParams.x;

	distance = SAMPLE_TEXTURE2D_LOD(ShadowMap, PointClamp, shadowCoord.xy, 0);

	// Shadow coords that fall out of the light frustum volume must always return attenuation 1.0
	// TODO: We could use branch here to save some perf on some platforms.
	return BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : distance;
}
#endif

void MainLightBlurShadow_half(half RandSeed, half FadeTightness, SamplerState PointClamp, half BlurRadius, half3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
#if SHADERGRAPH_PREVIEW
	Direction = half3(0.5, 0.5, 0);
	Color = 1;
	DistanceAtten = 1;
	ShadowAtten = 1;
#else
	Light mainLight = GetMainLight();
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAtten = mainLight.distanceAttenuation;
	ShadowAtten = 1;
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half4 shadowParams = GetMainLightShadowParams();
	for (int i = 1; i < 41; i++)
	{
		half3 newWorldPos = WorldPos + half3(sin(RandSeed + i), 0, cos(RandSeed + i)) * i * 0.025 * BlurRadius;
#if SHADOWS_SCREEN
		half4 clipPos = TransformWorldToHClip(newWorldPos);
		half4 shadowCoord = ComputeScreenPos(clipPos);
#else
		half4 shadowCoord = TransformWorldToShadowCoord(newWorldPos);
#endif
		//mainLight = GetMainLight(shadowCoord);
		float shadowMapSample = SampleShadowmapDirect(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), PointClamp, shadowCoord, shadowSamplingData, shadowParams, false);

		float shadowCoordPos = shadowCoord.z;
		float shadowMapPos = shadowMapSample;
		float fade = ((shadowMapPos - shadowCoordPos) * FadeTightness);
		float onedivfade = 1 / min(fade, 0.7);
		ShadowAtten -= step(shadowCoord.z, shadowMapSample) * 0.0025 * lerp(max((41 - i* onedivfade)*onedivfade,0), i, saturate(fade));// *(1 - saturate(fade / 3));
		ShadowAtten = saturate(ShadowAtten);
	}
#endif
}