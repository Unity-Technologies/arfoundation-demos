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
    TMP_Text m_CurrentClassificationLabel;

    public TMP_Text currentClassificationLabel
    {
        get => m_CurrentClassificationLabel;
        set => m_CurrentClassificationLabel = value;
    }

    [SerializeField]
    ARMeshManager m_MeshManager;

    public ARMeshManager meshManager
    {
        get => m_MeshManager;
        set => m_MeshManager = value;
    }

    [SerializeField]
    public Camera m_MainCamera;

    public Camera mainCamera
    {
        get => m_MainCamera;
        set => m_MainCamera = value;
    }
    
    Vector2 m_ScreenCenter;
    RaycastHit m_Hit;
    TrackableId m_CurrentTrackableID;
    XRMeshSubsystem m_MeshSubsystem;
    ARMeshClassification m_CurrentClassification;

    public ARMeshClassification currentClassification => m_CurrentClassification;
    
    readonly Dictionary<TrackableId, NativeArray<ARMeshClassification>> m_MeshDictionary = new Dictionary<TrackableId, NativeArray<ARMeshClassification>>();

    void OnEnable()
    {
        m_ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        m_MeshSubsystem = m_MeshManager.subsystem;
        m_MeshSubsystem.SetClassificationEnabled(true);

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
            m_CurrentClassificationLabel.text = GetClassificationName(m_CurrentClassification);
        }
    }

    void MeshManagerOnmeshesChanged(ARMeshesChangedEventArgs obj)
    {
        foreach (MeshFilter mesh in obj.added)
        {
            TrackableId trackableId = ExtractTrackableId(mesh.name);
            m_MeshDictionary.Add(trackableId, m_MeshSubsystem.GetFaceClassifications(trackableId, Allocator.Persistent));
        }

        foreach (MeshFilter mesh in obj.updated)
        {
            TrackableId trackableId = ExtractTrackableId(mesh.name);

            // Must dispose of the memory allocated for the previous native array in the entry, before setting a new one.
            if (m_MeshDictionary[trackableId].IsCreated)
            {
                m_MeshDictionary[trackableId].Dispose();
            }

            m_MeshDictionary[trackableId] = m_MeshSubsystem.GetFaceClassifications(trackableId, Allocator.Persistent);
        }

        foreach (MeshFilter mesh in obj.removed)
        {
            TrackableId trackableId = ExtractTrackableId(mesh.name);

            // Must dispose of the memory allocated for the native array, before deleting the entry in the dictionary.
            if (m_MeshDictionary[trackableId].IsCreated)
            {
                m_MeshDictionary[trackableId].Dispose();
            }

            m_MeshDictionary.Remove(trackableId);
        }
    }

    void SetCurrentClassification(TrackableId meshID, int triangleIndex)
    {
        Debug.Assert(m_MeshDictionary.ContainsKey(meshID), $"Mesh ID [{meshID}] does not exist in the dictionary");
        Debug.Assert(triangleIndex < m_MeshDictionary[meshID].Length, $"Mesh ID [{meshID}] does have a triangle classification with index {triangleIndex}");
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
