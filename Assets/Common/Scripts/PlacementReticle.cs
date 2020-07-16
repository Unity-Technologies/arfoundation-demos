using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementReticle : MonoBehaviour
{
    [SerializeField]
    bool m_SnapToMesh;

    public bool snapToMesh
    {
        get => m_SnapToMesh;
        set => m_SnapToMesh = value;
    }

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    
    public ARRaycastManager raycastManager
    {
        get => m_RaycastManager;
        set => m_RaycastManager = value;
    }

    [SerializeField]
    GameObject m_ReticlePrefab;

    public GameObject reticlePrefab
    {
        get => m_ReticlePrefab;
        set => m_ReticlePrefab = value;
    }

    GameObject m_SpawnedReticle;
    CenterScreenHelper m_CenterScreen;
    TrackableType m_RaycastMask;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Start()
    {
        m_CenterScreen = CenterScreenHelper.Instance;
        if (m_SnapToMesh)
        {
            m_RaycastMask = TrackableType.PlaneEstimated;
        }
        else
        {
            m_RaycastMask = TrackableType.PlaneWithinPolygon;
        }

        m_SpawnedReticle = Instantiate(m_ReticlePrefab);
        m_SpawnedReticle.SetActive(false);
    }

    void Update()
    {
        if (m_RaycastManager.Raycast(m_CenterScreen.GetCenterScreen(), s_Hits, m_RaycastMask))
        {
            Pose hitPose = s_Hits[0].pose;
            m_SpawnedReticle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            m_SpawnedReticle.SetActive(true);
        }
    }

    public Transform GetReticlePosition()
    {
        // if not active ie: not snapping to a plane return null
        if (!m_SpawnedReticle.activeSelf)
        {
            return null;
        }
        
        return m_SpawnedReticle.transform;
    }
}
