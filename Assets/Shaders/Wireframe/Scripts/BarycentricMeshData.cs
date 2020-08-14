using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BarycentricMeshData : MonoBehaviour
{
    [SerializeField]
    ARMeshManager m_MeshManager;

    public ARMeshManager meshManager
    {
        get => m_MeshManager;
        set => m_MeshManager = value;
    }

    [SerializeField]
    BarycentricDataBuilder m_DataBuilder;

    public BarycentricDataBuilder dataBuilder
    {
        get => m_DataBuilder;
        set => m_DataBuilder = value;
    }

    List<MeshFilter> m_AddedMeshes = new List<MeshFilter>();
    List<MeshFilter> m_UpdatedMeshes = new List<MeshFilter>();

    void OnEnable()
    {
        m_MeshManager.meshesChanged += MeshManagerOnmeshesChanged;        
    }

    void OnDisable()
    {
        m_MeshManager.meshesChanged -= MeshManagerOnmeshesChanged;
    }

    void MeshManagerOnmeshesChanged(ARMeshesChangedEventArgs obj)
    {
        m_AddedMeshes = obj.added;
        m_UpdatedMeshes = obj.updated;
        
        
        foreach (MeshFilter filter in m_AddedMeshes)
        {
            m_DataBuilder.GenerateData(filter.mesh);
        }

        foreach (MeshFilter filter in m_UpdatedMeshes)
        {
            m_DataBuilder.GenerateData(filter.sharedMesh);
        }
/*
        if (obj.updated.Count > 0)
        {
            m_DataBuilder.GenerateData(obj.updated[0].sharedMesh);
        }
        */
    }
}
