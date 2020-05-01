using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class BodyRecordingUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Human body manager reference")]
    ARHumanBodyManager m_HumanBodyManager;

    public ARHumanBodyManager humanBodyManager
    {
        get => m_HumanBodyManager;
        set => m_HumanBodyManager = value;
    }

    [SerializeField]
    [Tooltip("Body tracking UI reference")]
    GameObject m_BodyTrackingUI;

    public GameObject bodyTrackingUI
    {
        get => m_BodyTrackingUI;
        set => m_BodyTrackingUI = value;
    }

    [SerializeField]
    [Tooltip("World tracking UI reference")]
    GameObject m_WorldTrackingUI;

    public GameObject worldTrackingUI
    {
        get => m_WorldTrackingUI;
        set => m_WorldTrackingUI = value;
    }

    [SerializeField]
    [Tooltip("Button for playing back captured animation")]
    Button m_AnimationPlayButton;

    public Button animationPlayButton
    {
        get => m_AnimationPlayButton;
        set => m_AnimationPlayButton = value;
    }
    
    BodyPlayback m_BodyPlayback;

    void OnEnable()
    {
        m_HumanBodyManager.humanBodiesChanged += HumanBodyManagerOnhumanHumanBodiesChanged;
        PlaceObjectsOnPlane.onPlacedObject += PlacedObject;
    }


    void OnDisable()
    {
        m_HumanBodyManager.humanBodiesChanged -= HumanBodyManagerOnhumanHumanBodiesChanged;
        PlaceObjectsOnPlane.onPlacedObject -= PlacedObject;
    }

    void HumanBodyManagerOnhumanHumanBodiesChanged(ARHumanBodiesChangedEventArgs obj)
    {
        
    }
    
    void PlacedObject()
    {
        m_BodyPlayback = FindObjectOfType<BodyPlayback>();
        m_AnimationPlayButton.onClick.AddListener(m_BodyPlayback.AnimationToggle);
    }

    public void ShowWorldTrackingUI()
    {
        m_BodyTrackingUI.SetActive(false);
        m_WorldTrackingUI.SetActive(true);
    }

    public void ShowBodyTrackingUI()
    {
        m_WorldTrackingUI.SetActive(false);
        m_BodyTrackingUI.SetActive(true);
    }
}
