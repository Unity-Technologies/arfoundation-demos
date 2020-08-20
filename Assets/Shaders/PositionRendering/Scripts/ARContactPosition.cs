using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARContactPosition : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    
    public ARRaycastManager raycastManager
    {
        get => m_RaycastManager;
        set => m_RaycastManager = value;
    }

    [SerializeField]
    ARPlaneManager m_PlaneManager;

    public ARPlaneManager planeManager
    {
        get => m_PlaneManager;
        set => m_PlaneManager = value;
    }

    [SerializeField]
    bool m_DelayPosition;

    public bool delayPosition
    {
        get => m_DelayPosition;
        set => m_DelayPosition = true;
    }
    
    Vector3 m_TargetPosition;

    public Vector3 targetPosition
    {
        get => m_TargetPosition;
        set => m_TargetPosition = value;
    }

    Quaternion m_TargetRotation;

    public Quaternion targetRotation
    {
        get => m_TargetRotation;
        set => m_TargetRotation = value;
    }
    
    TrackableId m_HitID;
    Pose m_HitPose;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    const string k_ShaderPositionProperty = "_ContactPosition";
    const string k_ShaderContactProperty = "_IsInContact";
    const float k_LerpDelayValue = 0.05f;

    void OnEnable()
    {
        m_TargetPosition = Vector3.zero;
    }

    void Update()
    {
        if (m_RaycastManager.Raycast(CenterScreenHelper.Instance.GetCenterScreen(), s_Hits, TrackableType.PlaneWithinBounds))
        {
            m_HitID = s_Hits[0].trackableId;
            m_HitPose = s_Hits[0].pose;
            m_TargetRotation = m_HitPose.rotation;
        }

        if (m_DelayPosition)
        {
            m_TargetPosition = Vector3.Lerp(m_TargetPosition, m_HitPose.position, k_LerpDelayValue);
        }
        else
        {
            m_TargetPosition = m_HitPose.position;
        }
        
        foreach(ARPlane plane in m_PlaneManager.trackables)
        {
            if (plane.trackableId == m_HitID)
            {
                plane.transform.GetComponent<MeshRenderer>().material.SetFloat(k_ShaderContactProperty, 1);
                plane.transform.GetComponent<MeshRenderer>().material.SetVector(k_ShaderPositionProperty, m_TargetPosition);
            }
            else
            {
                plane.transform.GetComponent<MeshRenderer>().material.SetFloat(k_ShaderContactProperty, 0);
            }
        }
    }
}
