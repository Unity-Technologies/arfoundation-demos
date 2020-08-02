// Based on: https://catlikecoding.com/unity/tutorials/advanced-rendering/flat-and-wireframe-shading/

using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BarycentricDataBuilder : MonoBehaviour
{
    private Mesh m_Mesh;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBarycentricData();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    GenerateBarycentricData();
    //}

    void GenerateBarycentricData()
    {
        m_Mesh = GetComponent<MeshFilter>().mesh;

        SplitMesh(m_Mesh);

        Color[] colorCoords = new[]
        {
            new Color(1, 0, 0),
            new Color(0, 1, 0),
            new Color(0, 0, 1),
        };

        Color32[] vertexColors = new Color32[m_Mesh.vertexCount];

        //compute vertexcolors
        //for(int i = 0; i < vertexColors.Length; i++)
        //{
        //    vertexColors[i] = colorCoords[(int)Mathf.Repeat(i, 3)];
        //    //vertexColors[i] = Color.red;
        //}

        for (int i = 0; i < vertexColors.Length; i += 3)
        {
            vertexColors[i] = colorCoords[0];
            vertexColors[i + 1] = colorCoords[1];
            vertexColors[i + 2] = colorCoords[2];
        }

        //for (int i = 0; i < m_Mesh.triangles.Length; i++)
        //{
        //    vertexColors[m_Mesh.triangles[i]] = colorCoords[(int)Mathf.Repeat(i, 3)];

        //    //vertexColors[m_Mesh.triangles[i]] = colorCoords[0];
        //    //vertexColors[m_Mesh.triangles[i + 1]] = colorCoords[1];
        //    //vertexColors[m_Mesh.triangles[i + 2]] = colorCoords[2];
        //}

        //compute vertexcolors

        m_Mesh.colors32 = vertexColors;

        //m_Mesh.RecalculateNormals();
        //m_Mesh.MarkModified();
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
}