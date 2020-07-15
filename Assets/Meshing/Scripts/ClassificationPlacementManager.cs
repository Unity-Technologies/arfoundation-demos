using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

public class ClassificationPlacementManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_FloorPrefabs;

    [SerializeField]
    List<GameObject> m_TablePrefabs;

    [SerializeField]
    List<GameObject> m_WallPrefabs;

    [SerializeField]
    MeshClassificationManager m_ClassificationManager;

    [SerializeField]
    PlacementReticle m_Reticle;

    Touch m_Touch;
    List<RaycastResult> m_OverUIResults = new List<RaycastResult>();

    [SerializeField]
    GameObject m_FloorUI;

    [SerializeField]
    GameObject m_WallUI;

    [SerializeField]
    GameObject m_TableUI;

    [SerializeField]
    Transform m_ARCameraTransform;

    bool m_ShowingSelectionUI;

    void Update()
    {
        if (m_ClassificationManager.currentClassification == ARMeshClassification.Table ||
            m_ClassificationManager.currentClassification == ARMeshClassification.Floor ||
            m_ClassificationManager.currentClassification == ARMeshClassification.Wall)
        {

            switch (m_ClassificationManager.currentClassification)
            {
                case ARMeshClassification.Floor:
                m_FloorUI.SetActive(true);
                m_WallUI.SetActive(false);
                m_TableUI.SetActive(false);
                break;
                
                case ARMeshClassification.Wall:
                m_WallUI.SetActive(true);
                m_FloorUI.SetActive(false);
                m_TableUI.SetActive(false);
                break;
                
                case ARMeshClassification.Table: 
                m_TableUI.SetActive(true);
                m_FloorUI.SetActive(false);
                m_WallUI.SetActive(false);
                break;
            }
            
        }
        else
        {
            m_FloorUI.SetActive(false);
            m_WallUI.SetActive(false);
            m_TableUI.SetActive(false);
            
        }
    }

    public void PlaceFloorObject(int indexToPlace)
    {
        GameObject m_SpawnedObject = Instantiate(m_FloorPrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        // look at device but stay 'flat'
        m_SpawnedObject.transform.LookAt(m_ARCameraTransform, Vector3.up);
        m_SpawnedObject.transform.rotation = Quaternion.Euler(0, m_SpawnedObject.transform.eulerAngles.y, 0);
    }

    public void PlaceWallObject(int indexToPlace)
    {
        Instantiate(m_WallPrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
    }

    public void PlaceTableObject(int indexToPlace)
    {
        GameObject m_SpawnedObject = Instantiate(m_TablePrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        // look at device but stay 'flat'
        m_SpawnedObject.transform.LookAt(m_ARCameraTransform, Vector3.up);
        m_SpawnedObject.transform.rotation = Quaternion.Euler(0, m_SpawnedObject.transform.eulerAngles.y, 0);
    }
}
