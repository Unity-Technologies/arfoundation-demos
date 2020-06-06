using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARUXReasonsManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text m_ReasonDisplayText;

    public TMP_Text reasonDisplayText
    {
        get => m_ReasonDisplayText;
        set => m_ReasonDisplayText = value;
    }
    
    NotTrackingReason m_CurrentReason;
    bool m_SessionTracking;

    const string k_InitRelocalText = "Session Initializing";
    const string k_MotionText = "Move the device slower";
    const string k_LightText = "Move to a brighter area";
    const string k_Features = "Not enough features";
    const string k_Unsupported = "AR is not supported on this device";
    const string k_None = "Error, not tracking";
    
    void OnEnable()
    {
        ARSession.stateChanged += ARSessionOnstateChanged;
    }

    void Update()
    {
        if (!m_SessionTracking)
        {
            m_CurrentReason = ARSession.notTrackingReason;
            ShowReason();
        }
        else
        {
            if (m_ReasonDisplayText.gameObject.activeSelf)
            {
                m_ReasonDisplayText.gameObject.SetActive(false);
            }
        }
    }

    void ARSessionOnstateChanged(ARSessionStateChangedEventArgs obj)
    {
        m_SessionTracking = obj.state == ARSessionState.SessionTracking ? true : false;
    }

    void ShowReason()
    {
        m_ReasonDisplayText.gameObject.SetActive(true);
        m_ReasonDisplayText.text = ReasonString();
    }

    string ReasonString()
    {
        string retVal = String.Empty;

        switch (m_CurrentReason)
        {
            case NotTrackingReason.Initializing:
            case NotTrackingReason.Relocalizing:
                retVal = k_InitRelocalText; 
                break;
            case NotTrackingReason.ExcessiveMotion:
                retVal = k_MotionText;
                break;
            case NotTrackingReason.InsufficientLight:
                retVal = k_LightText;
                break;
            case NotTrackingReason.InsufficientFeatures:
                retVal = k_Features;
                break;
            case NotTrackingReason.Unsupported:
                retVal = k_Unsupported;
                break;
            case NotTrackingReason.None:
                retVal = k_None;
                break;
        }

        return retVal;
    }
    
    
}
