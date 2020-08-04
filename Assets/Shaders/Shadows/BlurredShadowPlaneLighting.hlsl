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
	for (int i = 0; i < 40; i++)
	{
		half3 newWorldPos = WorldPos + half3(sin(RandSeed + i), 0, cos(RandSeed + i)) * i * 0.025 * BlurRadius;
#if SHADOWS_SCREEN
		half4 clipPos = TransformWorldToHClip(newWorldPos);
		half4 shadowCoord = ComputeScreenPos(clipPos);
#else
		half4 shadowCoord = TransformWorldToShadowCoord(newWorldPos);
#endif
#ifdef _MAIN_LIGHT_SHADOWS_CASCADE
		half cascadeIndex = ComputeCascadeIndex(newWorldPos);
#else
		half cascadeIndex = 0;
#endif
		float shadowScale = pow(_MainLightWorldToShadow[cascadeIndex], 0.6) * 20; // Some "Magic Math" to try to make transitions between cascades smoother
		//mainLight = GetMainLight(shadowCoord);
		float shadowMapSample = SampleShadowmapDirect(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), PointClamp, shadowCoord, shadowSamplingData, shadowParams, false);

		float shadowCoordPos = shadowCoord.z / shadowScale;
		float shadowMapPos = shadowMapSample / shadowScale;
		float fade = max(0.01, ((shadowMapPos - shadowCoordPos) * FadeTightness));
		float onedivfade = 1 / saturate(fade);
		ShadowAtten -= step(shadowCoord.z, shadowMapSample) * 0.025 * clamp((41 - i * onedivfade), 0, onedivfade*2) * max(step(fade * 8, i+1), 0.3);
		ShadowAtten = BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : saturate(ShadowAtten);
	}
#endif
}