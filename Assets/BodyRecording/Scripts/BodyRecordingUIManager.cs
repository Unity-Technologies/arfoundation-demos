using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class BodyRecordingUIManager : MonoBehaviour
{
    [SerializeField]
    ARHumanBodyManager m_HumanBodyManager;

    [SerializeField]
    GameObject m_BodyTrackingUI;

    [SerializeField]
    GameObject m_WorldTrackingUI;

    [SerializeField]
    Button m_AnimationPlayButton;
    
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
