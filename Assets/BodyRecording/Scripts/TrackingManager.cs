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
    [Tooltip("Current tracking state")]
    CurrentTrackingState m_TrackingState;

    public CurrentTrackingState trackingState
    {
        get => m_TrackingState;
        set => m_TrackingState = value;
    }
    
    [SerializeField]
    [Tooltip("Plane manager reference")]
    ARPlaneManager m_PlaneManager;
    
    public ARPlaneManager planeManager
    {
        get => m_PlaneManager;
        set => m_PlaneManager = value;
    }

    [SerializeField]
    [Tooltip("PointCloud manager reference")]
    ARPointCloudManager m_PointCloudManager;
    
    public ARPointCloudManager pointCloudManager
    {
        get => m_PointCloudManager;
        set => m_PointCloudManager = value;
    }

    [SerializeField]
    [Tooltip("Raycast manager reference")]
    ARRaycastManager m_RaycastManager;
    
    public ARRaycastManager raycastManager
    {
        get => m_RaycastManager;
        set => m_RaycastManager = value;
    }

    [SerializeField]
    [Tooltip("Human body manager reference")]
    ARHumanBodyManager m_HumanBodyManager;
    
    public ARHumanBodyManager humanBodyManager
    {
        get => m_HumanBodyManager;
        set => m_HumanBodyManager = value;
    }

    [SerializeField]
    [Tooltip("Place objects on plane reference")]
    PlaceObjectsOnPlane m_PlaceObjectsOnPlane;

    public PlaceObjectsOnPlane placeObjectsOnPlane
    {
        get => m_PlaceObjectsOnPlane;
        set => m_PlaceObjectsOnPlane = value;
    }

    /*
    [SerializeField]
    [Tooltip("UI manager reference")]
    UIManager m_UIManager;
    
    public UIManager uiManager
    {
        get => m_UIManger;
        set => m_UIManger = value;
    }
    
    bool m_FirstWorldTracking = true;
    */
    
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
            
            // destroy object that was placed in the scene
            if (FindObjectOfType<JointHandler>())
            {
                Destroy(FindObjectOfType<JointHandler>().gameObject);
            }
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
            
            Debug.Log("thing");

            // show instructional UI is enabling world tracking for the first time
            /*
            if (m_FirstWorldTracking)
            {
                // uses m_UIManager 
                if (m_UIManager)
                {
                    m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.CrossPlatformFindAPlane, UIManager.InstructionGoals.FoundAPlane));
                    m_UIManager.AddToQueue(new UXHandle(UIManager.InstructionUI.TapToPlace, UIManager.InstructionGoals.PlacedAnObject));
                }
                m_FirstWorldTracking = false;
            }
            */
        }
    }
}
