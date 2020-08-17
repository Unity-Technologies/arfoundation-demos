/*#ifndef SHADERGRAPH_PREVIEW
real SampleShadowmapDirect(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), SamplerState PointClamp, float4 shadowCoord, ShadowSamplingData samplingData, bool isPerspectiveProjection = true)
{

	// Compiler will optimize this branch away as long as isPerspectiveProjection is known at compile time
	if (isPerspectiveProjection)
		shadowCoord.xyz /= shadowCoord.w;

	real distance;

	// Instead of getting a value of 0 or 1 depending on a shadowmap comparison, we want to actually sample the shadowmap's values
	distance = SAMPLE_TEXTURE2D_LOD(ShadowMap, PointClamp, shadowCoord.xy, 0);
	
	//int smWidth, smHeight;
	//ShadowMap.GetDimensions(smWidth, smHeight);
	//shadowCoord = saturate(shadowCoord);
	//distance = ShadowMap[int2(smWidth * shadowCoord.x, smHeight * shadowCoord.y)];

	return distance;
}
#endif*/

void MainLightBlurShadow_half(half rand, half fadeTightness, SamplerState pointClamp, half blurRadius, half3 worldPos, out half shadowAtten)
{
#if SHADERGRAPH_PREVIEW
	shadowAtten = 1;
#else
	const int NUM_STEPS = 40;
	float oneDivNumSteps = 1.0 / NUM_STEPS;
	Light mainLight = GetMainLight();
	shadowAtten = 1;
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	for (int i = 0; i < NUM_STEPS; i++)
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
		float shadowScale = pow(_MainLightWorldToShadow[cascadeIndex], 0.7) * 20; // Some "Magic Math" to try to make transitions between cascades smoother
		//float shadowMapSample = SampleShadowmapDirect(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), pointClamp, shadowCoord, shadowSamplingData, false);
		float shadowMapSample = SAMPLE_TEXTURE2D_LOD(_MainLightShadowmapTexture, pointClamp, shadowCoord.xy, 0);

#if defined(SHADER_API_GLES3) || defined(SHADER_API_GLES) || defined(SHADER_API_GLCORE)
		shadowMapSample = 1 - shadowMapSample;
		shadowCoord.z = 1 - shadowCoord.z;
#endif
		float shadowCoordPos = shadowCoord.z / shadowScale; //Divide by shadowScale to get transitions more close-ish when using shadow cascades
		float shadowMapPos = shadowMapSample / shadowScale;
		//blurAmount is a value between 0 and 1 expressing how much to blur 
		float blurAmount = max(0.01, ((shadowMapPos - shadowCoordPos) * fadeTightness));
		float oneDivBlurAmount = 1 / saturate(blurAmount);
		//0 or 1 depending on whether a shadow is being cast
		float isInShadow = step(shadowCoord.z, shadowMapSample); 
		// reduce the influence of higher iterations (further out samples) if the sampled shadowmap value is close to our position
		float tightenCloseShadows = clamp((NUM_STEPS - i * oneDivBlurAmount), 0, oneDivBlurAmount * 2); 
		// reduce the influence of lower iterations (closer in samples) if the sampled shadowmap value is far way from our position
		float reduceInnerIterations = max(step(saturate(blurAmount) * NUM_STEPS / 4, i + 1), 0.3);
		shadowAtten -= isInShadow * oneDivNumSteps * tightenCloseShadows * reduceInnerIterations;
		// If the position is out of shadow distance, just return 1
		shadowAtten = BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : saturate(shadowAtten);
	}
#endif
}