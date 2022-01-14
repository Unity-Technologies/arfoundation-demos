// Code written by Landon Townsend

void PlaneContactShadow_float(float rand, float3 position, float dist, float depth, out float shadowAmount)
{
#if SHADERGRAPH_PREVIEW
	shadowAmount = 0;
#else
	const float STAR_ANGLE = 1.7;
	const int TOTAL_STEPS = 10;
	shadowAmount = 0;
	Light mainLight = GetMainLight();
	float3 direction = mainLight.direction;
	float3 dirX = cross(direction, float3(0, 1, 0)); // Unit vector at right angle to light direction
	dirX = length(dirX) == 0 ? float3(1, 0, 0) : normalize(dirX); //edge case incase the light direction is completely vertical
	float3 dirY = cross(direction, dirX); // Unit vector at right angle to the light direction and dirX
	for (int i = 0; i < TOTAL_STEPS; i++)
	{
		//"normal" is a vector that rotates STAR_ANGLE radians around the light direction every iteration; it faces opposite the light direction and 
		//is at a sharp angle with the light direction. This is to create the blurring effect. The starting orientation of the vector
		//is randomized using the "rand" input.
		float3 normal = -direction + (cos(i * STAR_ANGLE + rand) * dirX + sin(i * STAR_ANGLE + rand) * dirY) * 0.3;
		//by multiplying the step distance by the number of iterations (i) pixels that are further from the object will also recieve shadow, but less.
		float3 posStep = position - normal * i * 0.1 * dist;
		// Get the XY position of the projected position to be checked against the depth texture
		float4 newPos = mul(UNITY_MATRIX_VP, float4(posStep, 1.0));
		newPos /= newPos.w;
#if defined(SHADER_API_GLES3) || defined(SHADER_API_GLES) || defined(SHADER_API_GLCORE)
		newPos *= float4(0.5, 0.5, 1, 1);
		newPos += float4(0.5, 0.5, 0, 0);
#else
		newPos *= float4(0.5, -0.5, 1, 1);
		newPos += float4(0.5, 0.5, 0, 0);
#endif
		// Prevent the depth texture values from bleeding from one side to the other
		newPos = clamp(newPos, 0, 0.999);
		//Get the view space depth value of our projected point
		float3 newPosView = mul(UNITY_MATRIX_V, float4(posStep, 1.0));
		// Compare the view space depth of our projected point to the depth texture value at the same XY location
		float depthCompare = saturate(dist - abs(-newPosView.z - LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(newPos.xy), _ZBufferParams)));
		
		//scale by 2 divided by the total number of steps squared, the total number of steps minus i, and scale inversely by dist
		shadowAmount += 2.0 / (TOTAL_STEPS * TOTAL_STEPS) * (TOTAL_STEPS - i) * depthCompare / dist;
	}
#endif
}