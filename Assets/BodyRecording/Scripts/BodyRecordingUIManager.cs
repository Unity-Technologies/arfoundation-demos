using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BodyRecordingUIManager : MonoBehaviour
{
    [SerializeField]
    ARHumanBodyManager m_HumanBodyManager;

    [SerializeField]
    GameObject m_BodyRecordingUI;

    [SerializeField]
    GameObject m_BodyPlacingUI;

    void OnEnable()
    {
        m_HumanBodyManager.humanBodiesChanged += HumanBodyManagerOnhumanHumanBodiesChanged;
    }

    void OnDisable()
    {
        m_HumanBodyManager.humanBodiesChanged -= HumanBodyManagerOnhumanHumanBodiesChanged;
    }

    void HumanBodyManagerOnhumanHumanBodiesChanged(ARHumanBodiesChangedEventArgs obj)
    {
        
    }
}
