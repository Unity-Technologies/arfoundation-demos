using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public struct UXHandle
{
    public UIManager.InstructionUI InstructionalUI;
    public UIManager.InstructionGoals Goal;

    public UXHandle(UIManager.InstructionUI ui, UIManager.InstructionGoals goal)
    {
        InstructionalUI = ui;
        Goal = goal;
    }
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    bool m_StartWithInstructionalUI = true;

    public bool startWithInstructionalUI
    {
         get => m_StartWithInstructionalUI;
         set => m_StartWithInstructionalUI = value;
    }

    public enum InstructionUI
    {
        CrossPlatformFindAPlane,
        FindAFace,
        FindABody,
        FindAnImage,
        FindAnObject,
        ARKitCoachingOverlay,
        TapToPlace,
        None
    };

    [SerializeField]
    InstructionUI m_InstructionalUI;

    public InstructionUI instructionalUI
    {
        get => m_InstructionalUI;
        set => m_InstructionalUI = value;
    }

    public enum InstructionGoals
    {
        FoundAPlane,
        FoundMultiplePlanes,
        FoundAFace,
        FoundABody,
        FoundAnImage,
        FoundAnObject,
        PlacedAnObject,
        None
    };

    [SerializeField]
    InstructionGoals m_InstructionalGoal;
    
    public InstructionGoals instructionalGoal
    {
        get => m_InstructionalGoal;
        set => m_InstructionalGoal = value;
    }

    [SerializeField]
    bool m_ShowSecondaryInstructionalUI;
    
    public bool showSecondaryInstructionalUI
    {
        get => m_ShowSecondaryInstructionalUI;
        set => m_ShowSecondaryInstructionalUI = value;
    }

    [SerializeField]
    InstructionUI m_SecondaryInstructionUI = InstructionUI.TapToPlace;

    public InstructionUI secondaryInstructionUI
    {
        get => m_SecondaryInstructionUI;
        set => m_SecondaryInstructionUI = value;
    }

    [SerializeField]
    InstructionGoals m_SecondaryGoal = InstructionGoals.PlacedAnObject;

    public InstructionGoals secondaryGoal
    {
        get => m_SecondaryGoal;
        set => m_SecondaryGoal = value;
    }

    [SerializeField]
    [Tooltip("Fallback to cross platform UI if ARKit coaching overlay is not supported")]
    bool m_CoachingOverlayFallback;

    public bool coachingOverlayFallback
    {
        get => m_CoachingOverlayFallback;
        set => m_CoachingOverlayFallback = value;
    }

    [SerializeField]
    GameObject m_ARSessionOrigin;

    public GameObject arSessionOrigin
    {
        get => m_ARSessionOrigin;
        set => m_ARSessionOrigin = value;
    }

    Func<bool> m_GoalReached;
    bool m_SecondaryGoalReached;
    
    Queue<UXHandle> m_UXOrderedQueue;
    UXHandle m_CurrentHandle;
    bool m_ProcessingInstructions;
    bool m_PlacedObject;

    [SerializeField]
    ARPlaneManager m_PlaneManager;
    
    public ARPlaneManager planeManager
    {
        get => m_PlaneManager;
        set => m_PlaneManager = value;
    }

    [SerializeField]
    ARFaceManager m_FaceManager;
    public ARFaceManager faceManager
    {
        get => m_FaceManager;
        set => m_FaceManager = value;
    }

    [SerializeField]
    ARHumanBodyManager m_BodyManager;
    public ARHumanBodyManager bodyManager
    {
        get => m_BodyManager;
        set => m_BodyManager = value;
    }

    [SerializeField]
    ARTrackedImageManager m_ImageManager;
    public ARTrackedImageManager imageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    [SerializeField]
    ARTrackedObjectManager m_ObjectManager;

    public ARTrackedObjectManager objectManager
    {
        get => m_ObjectManager;
        set => m_ObjectManager = value;
    }

    [SerializeField]
    ARUXAnimationManager m_AnimationManager;

    public ARUXAnimationManager animationManager
    {
        get => m_AnimationManager;
        set => m_AnimationManager = value;
    }

    bool m_FadedOff = false;
    
    [SerializeField]
    LocalizationManager m_LocalizationManager;

    public LocalizationManager localizationManager
    {
        get => m_LocalizationManager;
        set => m_LocalizationManager = value;
    }

    void OnEnable()
    {
        ARUXAnimationManager.onFadeOffComplete += FadeComplete;

        PlaceObjectsOnPlane.onPlacedObject += () => m_PlacedObject = true;

        GetManagers();
        m_UXOrderedQueue = new Queue<UXHandle>();

        if (m_StartWithInstructionalUI)
        {
            m_UXOrderedQueue.Enqueue(new UXHandle(m_InstructionalUI, m_InstructionalGoal));
        }

        if (m_ShowSecondaryInstructionalUI)
        {
            m_UXOrderedQueue.Enqueue(new UXHandle(m_SecondaryInstructionUI, m_SecondaryGoal));
        }
    }

    void OnDisable()
    {
        ARUXAnimationManager.onFadeOffComplete -= FadeComplete;
    }

    void Update()
    {
        if (m_AnimationManager.localizeText)
        {
            if (!m_LocalizationManager.localizationComplete)
            {
                return;
            }
        }

        if (m_UXOrderedQueue.Count > 0 && !m_ProcessingInstructions)
        {
            // pop off
            m_CurrentHandle = m_UXOrderedQueue.Dequeue();
            
            // exit instantly, if the goal is already met it will skip showing the first UI and move to the next in the queue 
            m_GoalReached = GetGoal(m_CurrentHandle.Goal);
            if (m_GoalReached.Invoke())
            {
                return;
            }

            // fade on
            FadeOnInstructionalUI(m_CurrentHandle.InstructionalUI);
            m_ProcessingInstructions = true;
            m_FadedOff = false;
        }

        if (m_ProcessingInstructions)
        {
            // start listening for goal reached
            if (m_GoalReached.Invoke())
            {
                // if goal reached, fade off
                if (!m_FadedOff)
                {
                    m_FadedOff = true;
                    m_AnimationManager.FadeOffCurrentUI();
                }
            }
        }
    }

    void GetManagers()
    {
        if (m_ARSessionOrigin)
        {
            if (m_ARSessionOrigin.TryGetComponent(out ARPlaneManager arPlaneManager))
            {
                m_PlaneManager = arPlaneManager;
            }

            if (m_ARSessionOrigin.TryGetComponent(out ARFaceManager arFaceManager))
            {
                m_FaceManager = arFaceManager;
            }

            if (m_ARSessionOrigin.TryGetComponent(out ARHumanBodyManager arHumanBodyManager))
            {
                m_BodyManager = arHumanBodyManager;
            }

            if (m_ARSessionOrigin.TryGetComponent(out ARTrackedImageManager arTrackedImageManager))
            {
                m_ImageManager = arTrackedImageManager;
            }

            if (m_ARSessionOrigin.TryGetComponent(out ARTrackedObjectManager arTrackedObjectManager))
            {
                m_ObjectManager = arTrackedObjectManager;
            }
        }
    }
    
    Func<bool> GetGoal(InstructionGoals goal)
    {
        switch (goal)
        {
            case InstructionGoals.FoundAPlane:
                return PlanesFound;

            case InstructionGoals.FoundMultiplePlanes:
                return MultiplePlanesFound;

            case InstructionGoals.FoundABody:
                return BodyFound;

            case InstructionGoals.FoundAFace:
                return FaceFound;

            case InstructionGoals.FoundAnImage:
                return ImageFound;

            case InstructionGoals.FoundAnObject:
                return ObjectFound;

            case InstructionGoals.PlacedAnObject:
                return PlacedObject;

            case InstructionGoals.None:
                return () => false;
        }

        return () => false;
    }

    void FadeOnInstructionalUI(InstructionUI ui)
    {
        switch (ui)
        {
            case InstructionUI.CrossPlatformFindAPlane:
                m_AnimationManager.ShowCrossPlatformFindAPlane();
                break;

            case InstructionUI.FindAFace:
                m_AnimationManager.ShowFindFace();
                break;

            case InstructionUI.FindABody:
                m_AnimationManager.ShowFindBody();
                break;

            case InstructionUI.FindAnImage:
                m_AnimationManager.ShowFindImage();
                break;

            case InstructionUI.FindAnObject:
                m_AnimationManager.ShowFindObject();
                break;

            case InstructionUI.ARKitCoachingOverlay:
                if (m_AnimationManager.ARKitCoachingOverlaySupported())
                {
                    m_AnimationManager.ShowCoachingOverlay();
                }
                else
                {
                    // fall back to cross platform overlay
                    if (m_CoachingOverlayFallback)
                    {
                        m_AnimationManager.ShowCrossPlatformFindAPlane();
                    }
                }
                break;

            case InstructionUI.TapToPlace:
                m_AnimationManager.ShowTapToPlace();
                break;

            case InstructionUI.None:

                break;
        }
    }

    bool PlanesFound() => m_PlaneManager && m_PlaneManager.trackables.count > 0;

    bool MultiplePlanesFound() => m_PlaneManager && m_PlaneManager.trackables.count > 1;

    bool FaceFound() => m_FaceManager && m_FaceManager.trackables.count > 0;

    bool BodyFound() => m_BodyManager && m_BodyManager.trackables.count > 0;

    bool ImageFound() => m_ImageManager && m_ImageManager.trackables.count > 0;

    bool ObjectFound() => m_ObjectManager && m_ObjectManager.trackables.count > 0;
    
    void FadeComplete()
    {
        m_ProcessingInstructions = false;
    }

    bool PlacedObject()
    {
        // reset flag to be used multiple times
        if (m_PlacedObject)
        {
            m_PlacedObject = false;
            return true;
        }
        return m_PlacedObject;
    }

    public void AddToQueue(UXHandle uxHandle)
    {
        m_UXOrderedQueue.Enqueue(uxHandle);
    }

    public void TestFlipPlacementBool()
    {
        m_PlacedObject = true;
    }
}

