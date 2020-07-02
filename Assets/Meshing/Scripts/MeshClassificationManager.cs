using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

public class MeshClassificationManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text m_DebugText;

    [SerializeField]
    TMP_Text m_DebugTextTwo;

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    RaycastHit m_Hit;

    [SerializeField]
    ARMeshManager m_MeshManager;

    [SerializeField]
    Camera m_MainCamera;

    Vector2 m_ScreenCenter;

    ARMeshClassification m_CurrentClassification;

    TrackableId m_CurrentTrackableID;

    List<MeshFilter> m_AddedSubmeshes = new List<MeshFilter>();
    List<MeshFilter> m_UpdatedSubmeshes = new List<MeshFilter>();
    List<MeshFilter> m_RemovedSubmeshes = new List<MeshFilter>();
    XRMeshSubsystem m_MeshSubsystem;

    Dictionary<TrackableId, NativeArray<ARMeshClassification>> m_MeshDictionary;
    

    void OnEnable()
    {
        m_ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        m_MeshSubsystem = m_MeshManager.subsystem;
        m_MeshSubsystem.SetClassificationEnabled(true);
        
        m_MeshDictionary = new Dictionary<TrackableId, NativeArray<ARMeshClassification>>();
        
        m_MeshManager.meshesChanged += MeshManagerOnmeshesChanged;
    }

    void OnDisable()
    {
        m_MeshManager.meshesChanged -= MeshManagerOnmeshesChanged;
    }

    void Update()
    {
        if (Physics.Raycast(m_MainCamera.ScreenPointToRay(m_ScreenCenter), out m_Hit))
        {
            SetCurrentClassification(ExtractTrackableId(m_Hit.transform.name), m_Hit.triangleIndex);
            //m_DebugTextTwo.text = GetClassificationName(m_CurrentClassification);
        }
    }
    
    void MeshManagerOnmeshesChanged(ARMeshesChangedEventArgs obj)
    {
        m_AddedSubmeshes = obj.added; 
        m_UpdatedSubmeshes = obj.updated;
        m_RemovedSubmeshes = obj.removed;

        foreach (MeshFilter mesh in m_AddedSubmeshes)
        {
            m_CurrentTrackableID = ExtractTrackableId(mesh.name);
            m_MeshDictionary.Add(m_CurrentTrackableID, m_MeshSubsystem.GetFaceClassifications(m_CurrentTrackableID, Allocator.Persistent));
        }

        foreach (MeshFilter mesh in m_UpdatedSubmeshes)
        {
            m_CurrentTrackableID = ExtractTrackableId(mesh.name);
            m_MeshDictionary[m_CurrentTrackableID] = m_MeshSubsystem.GetFaceClassifications(m_CurrentTrackableID, Allocator.Persistent);
        }

        foreach (MeshFilter mesh in m_RemovedSubmeshes)
        {
            m_CurrentTrackableID = ExtractTrackableId(mesh.name);
            m_MeshDictionary.Remove(m_CurrentTrackableID);
        }
    }

    void SetCurrentClassification(TrackableId meshID, int triangleIndex)
    {
        m_CurrentClassification = m_MeshDictionary[meshID][triangleIndex];
    }

    string GetClassificationName(ARMeshClassification classification)
    {
        string retVal = String.Empty;
        switch (classification)
        {
            case ARMeshClassification.Ceiling:
                retVal = "Ceiling";
                break;
            case ARMeshClassification.Door:
                retVal = "Door";
                break;
            case ARMeshClassification.Floor:
                retVal = "Floor";
                break;
            case ARMeshClassification.None:
                retVal = "None";
                break;
            case ARMeshClassification.Seat:
                retVal = "Seat";
                break;
            case ARMeshClassification.Table:
                retVal = "Table";
                break;
            case ARMeshClassification.Wall:
                retVal = "Wall";
                break;
            case ARMeshClassification.Window:
                retVal = "Window";
                break;
        }
        return retVal;
    }
    
    TrackableId ExtractTrackableId(string meshFilterName)
    {
        string[] nameSplit = meshFilterName.Split(' ');
        return new TrackableId(nameSplit[1]);
    }
}
