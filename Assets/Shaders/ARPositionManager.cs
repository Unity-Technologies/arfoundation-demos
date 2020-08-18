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

    TrackableId m_HitID;
    Pose m_HitPose;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    const string k_ShaderPositionProperty = "_ContactPosition";
    const string k_ShaderContactProperty = "_IsInContact";

    void Update()
    {
        if (m_RaycastManager.Raycast(CenterScreenHelper.Instance.GetCenterScreen(), s_Hits, TrackableType.PlaneWithinBounds))
        {
            m_HitID = s_Hits[0].trackableId;
            m_HitPose = s_Hits[0].pose;
        }
        
        foreach(ARPlane plane in m_PlaneManager.trackables)
        {
            if (plane.trackableId == m_HitID)
            {
                plane.transform.GetComponent<MeshRenderer>().material.SetFloat(k_ShaderContactProperty, 1);
                plane.transform.GetComponent<MeshRenderer>().material.SetVector(k_ShaderPositionProperty, m_HitPose.position);
            }
            else
            {
                plane.transform.GetComponent<MeshRenderer>().material.SetFloat(k_ShaderContactProperty, 0);
            }
        }
    }
}
