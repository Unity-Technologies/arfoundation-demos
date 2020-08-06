Shader "Custom/ARCoreFogBackground"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _EnvironmentDepth("Texture", 2D) = "black" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "RenderType" = "Background"
            "ForceNoShadowCasting" = "True"
        }

        Pass
        {
            Cull Off
            ZTest Always
            ZWrite On
            Lighting Off
            LOD 100
            Tags
            {
                "LightMode" = "Always"
            }

            GLSLPROGRAM

            #pragma multi_compile_local __ ARCORE_ENVIRONMENT_DEPTH_ENABLED

            #pragma only_renderers gles3

            #include "UnityCG.glslinc"

#ifdef SHADER_API_GLES3
#extension GL_OES_EGL_image_external_essl3 : require
#endif // SHADER_API_GLES3

            // Device display transform is provided by the AR Foundation camera background renderer.
            uniform mat4 _UnityDisplayTransform;

#ifdef VERTEX
            varying vec2 textureCoord;

            void main()
            {
#ifdef SHADER_API_GLES3
                // Transform the position from object space to clip space.
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

                // Remap the texture coordinates based on the device rotation.
                textureCoord = (_UnityDisplayTransform * vec4(gl_MultiTexCoord0.x, 1.0f - gl_MultiTexCoord0.y, 1.0f, 0.0f)).xy;
#endif // SHADER_API_GLES3
            }
#endif // VERTEX

#ifdef FRAGMENT
            varying vec2 textureCoord;
            uniform samplerExternalOES _MainTex;

#ifdef ARCORE_ENVIRONMENT_DEPTH_ENABLED
            uniform sampler2D _EnvironmentDepth;
#endif // ARCORE_ENVIRONMENT_DEPTH_ENABLED

#if defined(SHADER_API_GLES3) && !defined(UNITY_COLORSPACE_GAMMA)
            float GammaToLinearSpaceExact (float value)
            {
                if (value <= 0.04045F)
                    return value / 12.92F;
                else if (value < 1.0F)
                    return pow((value + 0.055F)/1.055F, 2.4F);
                else
                    return pow(value, 2.2F);
            }

            vec3 GammaToLinearSpace (vec3 sRGB)
            {
                // Approximate version from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
                return sRGB * (sRGB * (sRGB * 0.305306011F + 0.682171111F) + 0.012522878F);

                // Precise version, useful for debugging, but the pow() function is too slow.
                // return vec3(GammaToLinearSpaceExact(sRGB.r), GammaToLinearSpaceExact(sRGB.g), GammaToLinearSpaceExact(sRGB.b));
            }

#endif // SHADER_API_GLES3 && !UNITY_COLORSPACE_GAMMA

            float ConvertDistanceToDepth(float d)
            {
                float zBufferParamsW = 1.0 / _ProjectionParams.y;
                float zBufferParamsY = _ProjectionParams.z * zBufferParamsW;
                float zBufferParamsX = 1.0 - zBufferParamsY;
                float zBufferParamsZ = zBufferParamsX * _ProjectionParams.w;

                // Clip any distances smaller than the near clip plane, and compute the depth value from the distance.
                return (d < _ProjectionParams.y) ? 1.0f : ((1.0 / zBufferParamsZ) * ((1.0 / d) - zBufferParamsW));
            }

            void main()
            {
#ifdef SHADER_API_GLES3
                vec3 result = texture(_MainTex, textureCoord).xyz;
                float depth = 1.0;
#ifndef UNITY_COLORSPACE_GAMMA
                result = GammaToLinearSpace(result);
#endif // !UNITY_COLORSPACE_GAMMA

#ifdef ARCORE_ENVIRONMENT_DEPTH_ENABLED
                float distance = texture(_EnvironmentDepth, textureCoord).x;
                depth = ConvertDistanceToDepth(distance);
#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)                
                UNITY_CALC_FOG_FACTOR(depth);
                UNITY_FOG_LERP_COLOR(result, unity_FogColor, unityFogFactor);
 #endif // defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)               
                
#endif // ARCORE_ENVIRONMENT_DEPTH_ENABLED

                gl_FragColor = vec4(result, 1.0);
                gl_FragDepth = depth;
#endif // SHADER_API_GLES3
            }

#endif // FRAGMENT
            ENDGLSL
        }
    }

    FallBack Off
}
