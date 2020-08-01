// Based on https://catlikecoding.com/unity/tutorials/advanced-rendering/flat-and-wireframe-shading/

Shader "AR/Wireframe"
{
    Properties
    {
        _WireThickness("Thickness", Range(0, 20)) = 1
        _DistanceToEdgeThreshold("Smoothing", Range(0,2)) = 0.01
        _WireColor("Color", Color) = (1.0,1.0,1.0,1.0)
    }
    
    SubShader
    {
        Tags
        {
            "LightMode" = "UniversalForward"
            "PassFlags" = "OnlyDirectional"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "WirePass"
            Blend SrcAlpha OneMinusSrcAlpha

             HLSLPROGRAM

             #pragma vertex vert
             #pragma fragment frag
             #pragma geometry geom

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float4 vertex : SV_POSITION;
                float4 dist : TEXCOORD2;
            };

            half _WireThickness;
            half _DistanceToEdgeThreshold;
            half4 _WireColor;

            v2g vert(appdata v)
            {
                v2g o;
                o.vertex = TransformObjectToHClip(v.vertex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2g i[3], inout TriangleStream<g2f> stream)
            {
                float2 p0 = i[0].vertex.xy / i[0].vertex.w;
                float2 p1 = i[1].vertex.xy / i[1].vertex.w;
                float2 p2 = i[2].vertex.xy / i[2].vertex.w;

                float2 edge0 = p2 - p1;
                float2 edge1 = p2 - p0;
                float2 edge2 = p1 - p0;

                float area = abs(edge1.x * edge2.y - edge1.y * edge2.x);
                float wireThickness = 20 - _WireThickness;

                g2f o;
                o.vertex = i[0].vertex;
                o.dist.xyz = float3((area / length(edge0)), 0.0, 0.0) * o.vertex.w * wireThickness;
                o.dist.w = 1.0 / o.vertex.w;
                stream.Append(o);

                o.vertex = i[1].vertex;
                o.dist.xyz = float3(0.0, (area / length(edge1)), 0.0) * o.vertex.w * wireThickness;
                o.dist.w = 1.0 / o.vertex.w;
                stream.Append(o);

                o.vertex = i[2].vertex;
                o.dist.xyz = float3(0.0, 0.0, (area / length(edge2))) * o.vertex.w * wireThickness;
                o.dist.w = 1.0 / o.vertex.w;
                stream.Append(o);
            }

            half4 frag(g2f i) : SV_Target
            {

                half4 col = _WireColor;// half4(0.0,0.0,0.0,0.0);

                float minDistanceToEdge = min(i.dist[0], min(i.dist[1], i.dist[2])) * i.dist[3];

                if (minDistanceToEdge > _DistanceToEdgeThreshold)
                {
                    col.a = 0.0;
                }
                else col.a = 1.0;

                return col;
            }

            ENDHLSL
        }
    }
}
