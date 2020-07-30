#ifndef SHADERGRAPH_PREVIEW
real SampleShadowmapNeverSoft(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, ShadowSamplingData samplingData, half4 shadowParams, bool isPerspectiveProjection = true)
{

	// Compiler will optimize this branch away as long as isPerspectiveProjection is known at compile time
	if (isPerspectiveProjection)
		shadowCoord.xyz /= shadowCoord.w;

	real attenuation;
	real shadowStrength = shadowParams.x;

	attenuation = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz);

	attenuation = LerpWhiteTo(attenuation, shadowStrength);

	// Shadow coords that fall out of the light frustum volume must always return attenuation 1.0
	// TODO: We could use branch here to save some perf on some platforms.
	return BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : attenuation;
}
#endif

void MainLightBlurShadow_half(half RandSeed, half BlurRadius, half3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
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
	ShadowAtten = 0;
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	half4 shadowParams = GetMainLightShadowParams();
	for (int i = 4; i < 24; i++)
	{
		half3 newWorldPos = WorldPos + half3(sin(RandSeed + i), 0, cos(RandSeed + i)) * i * 0.05 * BlurRadius;
#if SHADOWS_SCREEN
		half4 clipPos = TransformWorldToHClip(newWorldPos);
		half4 shadowCoord = ComputeScreenPos(clipPos);
#else
		half4 shadowCoord = TransformWorldToShadowCoord(newWorldPos);
#endif
		//mainLight = GetMainLight(shadowCoord);
		ShadowAtten += SampleShadowmapNeverSoft(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);
	}
	ShadowAtten *= 0.05;
#endif
}