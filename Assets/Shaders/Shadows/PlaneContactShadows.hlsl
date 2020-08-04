void PlaneContactShadow_float(float rand, float3 Position, float dist, float depth, out float shadowAmount)
{
#if SHADERGRAPH_PREVIEW
	shadowAmount = 0;
#else
	const float STAR_ANGLE = 1.7;
	shadowAmount = 0;
	Light mainLight = GetMainLight();
	float3 Direction = mainLight.direction;
	float3 dirX = normalize(cross(Direction, float3(0, 1, 0)));
	float3 dirY = cross(Direction, dirX);
	for (int i = 0; i < 10; i++)
	{
		//float3 Normal = float3(cos(i * STAR_ANGLE + rand), 0, sin(i * STAR_ANGLE + rand));
		float3 Normal = -Direction + (cos(i * STAR_ANGLE + rand) * dirX + sin(i * STAR_ANGLE + rand) * dirY) * 0.3;
		float3 posStep = Position - Normal * (i) * 0.1 * dist;
		float4 newPos = mul(UNITY_MATRIX_VP, float4(posStep, 1.0));
		newPos /= newPos.w;
		newPos *= float4(0.5, -0.5, 1, 1);
		newPos += float4(0.5, 0.5, 0, 0);
		float3 newPosView = mul(UNITY_MATRIX_V, float4(posStep, 1.0));
		newPos = clamp(newPos, 0, 0.999);
		shadowAmount += 0.02 / dist * (10 - i) * saturate(dist - abs(-newPosView.z - LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(newPos.xy), _ZBufferParams)));
	}
#endif
}