using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPositionManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    ARPlaneManager m_PlaneManager;

    [SerializeField]
    GameObject m_ObjectPrefab;

    GameObject m_SpawnedObject;
    Vector3 m_DelayedPosition;
    bool m_Placed = false;

    TrackableId m_HitID;
    Pose m_HitPose;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    const string k_ShaderPositionProperty = "_ContactPosition";
    const string k_ShaderContactProperty = "_IsInContact";

    void OnEnable()
    {
        m_SpawnedObject = Instantiate(m_ObjectPrefab);
        m_DelayedPosition = new Vector3(0,0,0);
    }

    public void SpawnPlaceableObject()
    {
        
    }
    
    void Update()
    {
        if (m_RaycastManager.Raycast(CenterScreenHelper.Instance.GetCenterScreen(), s_Hits, TrackableType.PlaneWithinBounds))
        {
            m_HitID = s_Hits[0].trackableId;
            m_HitPose = s_Hits[0].pose;
        }

        m_DelayedPosition = Vector3.Lerp(m_DelayedPosition, m_HitPose.position, 0.085f);

        if (!m_Placed)
        {
            m_SpawnedObject.transform.position = m_DelayedPosition;
        }

        foreach(ARPlane plane in m_PlaneManager.trackables)
        {
            if (plane.trackableId == m_HitID)
            {
                plane.transform.GetComponent<MeshRenderer>().material.SetFloat(k_ShaderContactProperty, 1);
                plane.transform.GetComponent<MeshRenderer>().material.SetVector(k_ShaderPositionProperty, m_DelayedPosition);
            }
            else
            {
                plane.transform.GetComponent<MeshRenderer>().material.SetFloat(k_ShaderContactProperty, 0);
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                m_Placed = true;
                m_SpawnedObject.GetComponent<AnimatedPlaceObject>().AnimatePlacement();
            }
        }
    }
}
