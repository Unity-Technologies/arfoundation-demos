// Based on: https://catlikecoding.com/unity/tutorials/advanced-rendering/flat-and-wireframe-shading/
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BarycentricDataBuilder : MonoBehaviour
{
    void Start()
    {
        //GenerateBarycentricData();
        GenerateUVData();
    }

    private void Reset()
    {
        GenerateBarycentricData();
    }

    void GenerateBarycentricData()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        SplitMesh(mesh);

        SetVertexColors(mesh);
    }

    void GenerateUVData()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        GenerateUVs(mesh);
    }

    public void GenerateData(Mesh mesh)
    {
        SplitMesh(mesh);

        SetVertexColors(mesh);
    }

    void SetVertexColors(Mesh mesh)
    {
        Color[] colorCoords = new[]
        {
            new Color(1, 0, 0),
            new Color(0, 1, 0),
            new Color(0, 0, 1),
        };

        Color32[] vertexColors = new Color32[mesh.vertices.Length];

        for (int i = 0; i < vertexColors.Length; i += 3)
        {
            vertexColors[i] = colorCoords[0];
            vertexColors[i + 1] = colorCoords[1];
            vertexColors[i + 2] = colorCoords[2];
        }

        mesh.colors32 = vertexColors;
    }

    void SplitMesh(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;

        Vector3[] newVerts;
        Vector3[] newNormals;
        Vector2[] newUvs;

        int n = triangles.Length;
        newVerts = new Vector3[n];
        newNormals = new Vector3[n];
        newUvs = new Vector2[n];

        for (int i = 0; i < n; i++)
        {
            newVerts[i] = verts[triangles[i]];
            newNormals[i] = normals[triangles[i]];
            if (uvs.Length > 0)
            {
                newUvs[i] = uvs[triangles[i]];
            }
            triangles[i] = i;
        }

        mesh.vertices = newVerts;
        mesh.normals = newNormals;
        mesh.uv = newUvs;
        mesh.triangles = triangles;
    }

    static List<Vector3> s_Vertices = new List<Vector3>();
    static List<Vector3> s_UVs = new List<Vector3>();
    [SerializeField]
    private float m_VertexDistance;

    public void GenerateUVs(Mesh mesh)
    {
        int vertexCount = mesh.vertexCount;

        s_UVs.Clear();
        if (s_UVs.Capacity < vertexCount) { s_UVs.Capacity = vertexCount; }

        mesh.GetVertices(s_Vertices);

        Vector3 centerInPlaneSpace = s_Vertices[s_Vertices.Count - 1];
        Vector3 uv = new Vector3(0, 0, 0);
        float shortestUVMapping = float.MaxValue;

        // Assume the last vertex is the center vertex.
        for (int i = 0; i < vertexCount - 1; i++)
        {
            float vertexDist = Vector3.Distance(s_Vertices[i], centerInPlaneSpace);

            // Remap the UV so that a UV of "1" marks the feathering boudary.
            // The ratio of featherBoundaryDistance/edgeDistance is the same as featherUV/edgeUV.
            // Rearrange to get the edge UV.
            float uvMapping = vertexDist / Mathf.Max(vertexDist-0.2f, 0.001f);
            uv.x = vertexDist/uvMapping;

            // All the UV mappings will be different. In the shader we need to know the UV value we need to fade out by.
            // Choose the shortest UV to guarentee we fade out before the border.
            // This means the feathering widths will be slightly different, we again rely on a fairly uniform plane.
            //if (shortestUVMapping > uvMapping) 
            //{ 
            //    shortestUVMapping = uvMapping; 
            //}

            s_UVs.Add(uv);
        }

        uv.Set(0, 0, 0);
        s_UVs.Add(uv);

        mesh.SetUVs(0, s_UVs);
        mesh.UploadMeshData(false);
    }
}