using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARUXReasonsManager : MonoBehaviour
{
    [SerializeField]
    bool m_ShowNotTrackingReasons = true;
    
    public bool showNotTrackingReasons
    {
        get => m_ShowNotTrackingReasons;
        set => m_ShowNotTrackingReasons = value;
    }

    [SerializeField]
    TMP_Text m_ReasonDisplayText;

    public TMP_Text reasonDisplayText
    {
        get => m_ReasonDisplayText;
        set => m_ReasonDisplayText = value;
    }

    [SerializeField]
    GameObject m_ReasonParent;
    
    public GameObject reasonParent
    {
        get => m_ReasonParent;
        set => m_ReasonParent = value;
    }

    [SerializeField]
    Image m_ReasonIcon;
    
    public Image reasonIcon
    {
        get => m_ReasonIcon;
        set => m_ReasonIcon = value;
    }

    [SerializeField]
    Sprite m_InitRelocalSprite;

    public Sprite initRelocalSprite
    {
        get => m_InitRelocalSprite;
        set => m_InitRelocalSprite = value;
    }

    [SerializeField]
    Sprite m_MotionSprite;

    public Sprite motionSprite
    {
        get => m_MotionSprite;
        set => m_MotionSprite = value;
    }

    [SerializeField]
    Sprite m_LightSprite;

    public Sprite lightSprite
    {
        get => m_LightSprite;
        set => m_LightSprite = value;
    }

    [SerializeField]
    Sprite m_FeaturesSprite;

    public Sprite featuresSprite
    {
        get => m_FeaturesSprite;
        set => m_FeaturesSprite = value;
    }

    [SerializeField]
    Sprite m_UnsupportedSprite;

    public Sprite unsupportedSprite
    {
        get => m_UnsupportedSprite;
        set => m_UnsupportedSprite = value;
    }

    [SerializeField]
    Sprite m_NoneSprite;

    public Sprite noneSprite
    {
        get => m_NoneSprite;
        set => m_NoneSprite = value;
    }

    [SerializeField]
    LocalizationManager m_LocalizationManager;
    
    public LocalizationManager localizationManager
    {
        get => m_LocalizationManager;
        set => m_LocalizationManager = value;
    }

    [SerializeField]
    bool m_LocalizeText = true;

    public bool localizeText
    {
        get => m_LocalizeText;
        set => m_LocalizeText = value;
    }

    NotTrackingReason m_CurrentReason;
    bool m_SessionTracking;

    const string k_InitRelocalText = "Initializing augmented reality.";
    const string k_MotionText = "Try moving at a slower pace.";
    const string k_LightText = "It’s too dark. Try going to a more well lit area.";
    const string k_FeaturesText = "Look for more textures or details in the area.";
    const string k_UnsupportedText = "AR content is not supported.";
    const string k_NoneText = "Wait for tracking to begin.";
    
    void OnEnable()
    {
        ARSession.stateChanged += ARSessionOnstateChanged;
        if (!m_ShowNotTrackingReasons)
        {
            m_ReasonParent.SetActive(false);
        }
    }

    void OnDisable()
    {
        ARSession.stateChanged -= ARSessionOnstateChanged;
    }

    void Update()
    {
        if (m_ShowNotTrackingReasons)
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
                    m_ReasonParent.SetActive(false);
                }
            }
        }
    }

    void ARSessionOnstateChanged(ARSessionStateChangedEventArgs obj)
    {
        m_SessionTracking = obj.state == ARSessionState.SessionTracking ? true : false;
    }

    void ShowReason()
    {
        m_ReasonParent.SetActive(true);
        SetReason();
    }

    void SetReason()
    {
        switch (m_CurrentReason)
        {
            case NotTrackingReason.Initializing:
            case NotTrackingReason.Relocalizing:
                if (m_LocalizeText)
                {
                    m_ReasonDisplayText.text = m_LocalizationManager.localizedInit;
                }
                else
                {
                    m_ReasonDisplayText.text = k_InitRelocalText;
                }
                m_ReasonIcon.sprite = m_InitRelocalSprite;
                break;
            case NotTrackingReason.ExcessiveMotion:
                if (m_LocalizeText)
                {
                    m_ReasonDisplayText.text = m_LocalizationManager.localizedMotion;
                }
                else
                {
                    m_ReasonDisplayText.text = k_MotionText;
                }
                m_ReasonIcon.sprite = m_MotionSprite;
                break;
            case NotTrackingReason.InsufficientLight:
                if(m_LocalizeText)
                {
                    m_ReasonDisplayText.text = m_LocalizationManager.localizedLight;
                }
                else
                {
                    m_ReasonDisplayText.text = k_LightText;
                }
                m_ReasonIcon.sprite = m_LightSprite;
                break;
            case NotTrackingReason.InsufficientFeatures:
                if (m_LocalizeText)
                {
                    m_ReasonDisplayText.text = m_LocalizationManager.localizedFeatures;
                }
                else
                {
                    m_ReasonDisplayText.text = k_FeaturesText;
                }
                m_ReasonIcon.sprite = m_FeaturesSprite;
                break;
            case NotTrackingReason.Unsupported:
                if (m_LocalizeText)
                {
                    m_ReasonDisplayText.text = m_LocalizationManager.localizedUnsupported;
                }
                else
                {
                    m_ReasonDisplayText.text = k_UnsupportedText;
                }
                m_ReasonIcon.sprite = m_UnsupportedSprite;
                break;
            case NotTrackingReason.None:
                if (m_LocalizeText)
                {
                    m_ReasonDisplayText.text = m_LocalizationManager.localizedNone;
                }
                else
                {
                    m_ReasonDisplayText.text = k_NoneText;
                }
                m_ReasonIcon.sprite = m_NoneSprite;
                break;
        }
    }

    public void TestForceShowReason(NotTrackingReason reason)
    {
        m_CurrentReason = reason;
        m_ReasonParent.SetActive(true);
        SetReason();
    }
    
}
