using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackingManager : MonoBehaviour
{
    public enum CurrentTrackingState
    {
        BodyTracking,
        WorldTracking
    }

    [SerializeField]
    CurrentTrackingState m_TrackingState;
    
    [SerializeField]
    ARPlaneManager m_PlaneManager;

    [SerializeField]
    ARPointCloudManager m_PointCloudManager;

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    ARHumanBodyManager m_HumanBodyManager;

    [SerializeField]
    PlaceObjectsOnPlane m_PlaceObjectsOnPlane;

    public void SwitchToWorldTracking()
    {
        SwitchTracking(CurrentTrackingState.WorldTracking);
    }

    public void SwitchToBodyTracking()
    {
        SwitchTracking(CurrentTrackingState.BodyTracking);
    }

    void SwitchTracking(CurrentTrackingState newTrackingState)
    {
        if (newTrackingState == CurrentTrackingState.BodyTracking)
        {
            // clean up tracked stuff
            m_PlaneManager.SetTrackablesActive(false);
            m_PlaneManager.enabled = false;
            
            m_PointCloudManager.SetTrackablesActive(false);
            m_PointCloudManager.enabled = false;

            m_RaycastManager.enabled = false;
            m_PlaceObjectsOnPlane.enabled = false;

            m_HumanBodyManager.enabled = true;
            m_TrackingState = CurrentTrackingState.BodyTracking;
            
            //TODO destroy placed object
        }
        else
        {
            // clean up and remove tracked bodies
            m_HumanBodyManager.SetTrackablesActive(false);
            m_HumanBodyManager.enabled = false;
            
            // enable subsystems for world tracking
            m_PlaneManager.enabled = true;
            m_PlaneManager.SetTrackablesActive(true);
            
            m_PointCloudManager.enabled = true;
            m_PointCloudManager.SetTrackablesActive(true);
            
            m_RaycastManager.enabled = true;
            m_PlaceObjectsOnPlane.enabled = true;
            m_TrackingState = CurrentTrackingState.WorldTracking;
        }
    }
}
