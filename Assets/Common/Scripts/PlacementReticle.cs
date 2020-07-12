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

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    GameObject m_ReticlePrefab;

    GameObject m_SpawnedReticle;
    CenterScreenHelper m_CenterScreen;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    TrackableType m_RaycastMask;

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
        else
        {
            return m_SpawnedReticle.transform;
        }
    }
}
