// Code written by Landon Townsend

void MainLightBlurShadow_half(half rand, half fadeTightness, SamplerState pointClamp, half blurRadius, half3 worldPos, out half shadowAtten)
{
#if SHADERGRAPH_PREVIEW
	shadowAtten = 1;
#else
	const int NUM_STEPS = 10;
	float oneDivNumSteps = 1.0 / NUM_STEPS;
	Light mainLight = GetMainLight();
	shadowAtten = 1;
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	for (int i = NUM_STEPS / 4; i < NUM_STEPS; i++)
	{
		//Instead of sampling the position of the current pixel, sample a point that's a i steps away, in a semi-randomized direction
		//the starting direction is randomized and then the direction is rotated by "i" radians every iteration
		half3 newWorldPos = worldPos + half3(sin(rand + i), 0, cos(rand + i)) * i * oneDivNumSteps * blurRadius;
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
#if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
		float shadowMapSample = 1.0;
#else
		float shadowMapSample = SAMPLE_TEXTURE2D_SHADOW(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture, shadowCoord.xyz);
#endif		
		//0 or 1 depending on whether a shadow is being cast
		float isInShadow = 1 - shadowMapSample;

		shadowAtten -= isInShadow * oneDivNumSteps;
		// If the position is out of shadow distance, just return 1
		shadowAtten = BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : saturate(shadowAtten);
	}
#endif
}