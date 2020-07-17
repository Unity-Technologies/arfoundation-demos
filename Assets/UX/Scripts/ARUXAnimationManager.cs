using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ARUXAnimationManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instructional test for visual UI")]
    TMP_Text m_InstructionText;
    
    /// <summary>
    /// Get the <c>Instructional Text</c>
    /// </summary>
    public TMP_Text instructionText
    {
        get => m_InstructionText;
        set => m_InstructionText = value;
    }

    [SerializeField]
    [Tooltip("Move device animation")]
    VideoClip m_FindAPlaneClip;

    /// <summary>
    /// Get the <c>Move device Clip</c>
    /// </summary>
    public VideoClip findAPlaneClip
    {
        get => m_FindAPlaneClip;
        set => m_FindAPlaneClip = value;
    }

    [SerializeField]
    [Tooltip("Tap to place animation")]
    VideoClip m_TapToPlaceClip;
    
    /// <summary>
    /// Get the <c>Tap to place Clip</c>
    /// </summary>
    public VideoClip tapToPlaceClip
    {
        get => m_TapToPlaceClip;
        set => m_TapToPlaceClip = value;
    }

    [SerializeField]
    [Tooltip("Find Clip animation")]
    VideoClip m_FindImageClip;
    
    /// <summary>
    /// Get the <c>Find Image Clip</c>
    /// </summary>
    public VideoClip findImageClip
    {
        get => m_FindImageClip;
        set => m_FindImageClip = value;
    }

    [SerializeField]
    [Tooltip("Find body animation")]
    VideoClip m_FindBodyClip;
    
    /// <summary>
    /// Get the <c>Find body Clip</c>
    /// </summary>
    public VideoClip findBodyClip
    {
        get => m_FindBodyClip;
        set => m_FindBodyClip = value;
    }

    [SerializeField]
    [Tooltip("Find object animation")]
    VideoClip m_FindObjectClip;
    
    /// <summary>
    /// Get the <c>Find object Clip</c>
    /// </summary>
    public VideoClip findObjectClip
    {
        get => m_FindObjectClip;
        set => m_FindObjectClip = value;
    }

    [SerializeField]
    [Tooltip("Find face animation")]
    VideoClip m_FindFaceClip;
    
    /// <summary>
    /// Get the <c>Find face iamge</c>
    /// </summary>
    public VideoClip findFaceClip
    {
        get => m_FindFaceClip;
        set => m_FindFaceClip = value;
    }

    [SerializeField]
    [Tooltip("ARKit Coaching overlay reference")]
    ARKitCoachingOverlay m_CoachingOverlay;

    /// <summary>
    /// Get the <c>ARKit coaching overlay</c>
    /// </summary>
    public ARKitCoachingOverlay coachingOverlay
    {
        get => m_CoachingOverlay;
        set => m_CoachingOverlay = value;
    }

    [SerializeField]
    [Tooltip("Video player reference")]
    VideoPlayer m_VideoPlayer;
    
    public VideoPlayer videoPlayer
    {
        get => m_VideoPlayer;
        set => m_VideoPlayer = value;
    }

    [SerializeField]
    [Tooltip("Raw image used for videoplayer reference")]
    RawImage m_RawImage;

    public RawImage rawImage
    {
        get => m_RawImage;
        set => m_RawImage = value;
    }

    [SerializeField]
    [Tooltip("time the UI takes to fade on")]
    float m_FadeOnDuration = 1.0f;
    [SerializeField]
    [Tooltip("time the UI takes to fade off")]
    float m_FadeOffDuration = 0.5f;
    
    Color m_AlphaWhite = new Color(1,1,1,0);
    Color m_White = new Color(1,1,1,1);

    Color m_TargetColor;
    Color m_StartColor;
    Color m_LerpingColor;
    bool m_FadeOn;
    bool m_FadeOff;
    bool m_Tweening;
    bool m_UsingARKitCoaching;
    float m_TweenTime;
    float m_TweenDuration;

    const string k_MoveDeviceText = "Move Device Slowly";
    const string k_TapToPlaceText = "Tap to Place AR";
    const string k_FindABodyText = "Find a Body to Track";
    const string k_FindAFaceText = "Find a Face to Track";
    const string k_FindClipText = "Find an Image to Track";
    const string k_FindObjectText = "Find an Object to Track";

    public static event Action onFadeOffComplete;

    [SerializeField]
    Texture m_Transparent;

    public Texture transparent
    {
        get => m_Transparent;
        set => m_Transparent = value;
    }

    RenderTexture m_RenderTexture;
    
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

    void Start()
    {
        m_StartColor = m_AlphaWhite;
        m_TargetColor = m_White;
    }

    void Update()
    {
        if (!m_VideoPlayer.isPrepared)
        {
            return;
        }

        if (m_FadeOff || m_FadeOn)
        {
            if (m_FadeOn)
            {
                m_StartColor = m_AlphaWhite;
                m_TargetColor = m_White;
                m_TweenDuration = m_FadeOnDuration;
                m_FadeOff = false;
            }
        
            if(m_FadeOff)
            {
                m_StartColor = m_White;
                m_TargetColor = m_AlphaWhite;
                m_TweenDuration = m_FadeOffDuration;

                m_FadeOn = false;
            }
            
            if (m_TweenTime < 1)
            {
                m_TweenTime += Time.deltaTime / m_TweenDuration;
                m_LerpingColor = Color.Lerp(m_StartColor, m_TargetColor, m_TweenTime);
                m_RawImage.color = m_LerpingColor;
                m_InstructionText.color = m_LerpingColor;
                
                m_Tweening = true;
            }
            else
            {
                m_TweenTime = 0;
                m_FadeOff = false;
                m_FadeOn = false;
                m_Tweening = false;
 
                // was it a fade off?
                if (m_TargetColor == m_AlphaWhite)
                {
                    if (onFadeOffComplete != null)
                    {
                        onFadeOffComplete();
                    }

                    // fix issue with render texture showing a single frame of the previous video
                    m_RenderTexture = m_VideoPlayer.targetTexture;
                    m_RenderTexture.DiscardContents();
                    m_RenderTexture.Release();
                    Graphics.Blit(m_Transparent, m_RenderTexture);
                }
            }
        }
    }
    
    public void ShowTapToPlace()
    {
        m_VideoPlayer.clip = m_TapToPlaceClip;
        m_VideoPlayer.Play();
        if (m_LocalizeText)
        {
            m_InstructionText.text = m_LocalizationManager.localizedTapToPlace;
        }
        else
        {
            m_InstructionText.text = k_TapToPlaceText;
        }
        
        m_FadeOn = true;
    }

    public void ShowFindImage()
    {
        m_VideoPlayer.clip = m_FindImageClip;
        m_VideoPlayer.Play();
        if (m_LocalizeText)
        {
            m_InstructionText.text = m_LocalizationManager.localizedImage;
        }
        else
        {
            m_InstructionText.text = k_FindClipText;
        }
        m_FadeOn = true;
    }

    public void ShowFindBody()
    {
        m_VideoPlayer.clip = m_FindBodyClip;
        m_VideoPlayer.Play();
        if (m_LocalizeText)
        {
            m_InstructionText.text = m_LocalizationManager.localizedBody;
        }
        else
        {
            m_InstructionText.text = k_FindABodyText;
        }    
        m_FadeOn = true;
        
    }

    public void ShowFindObject()
    {
        m_VideoPlayer.clip = m_FindObjectClip;
        m_VideoPlayer.Play();
        if (m_LocalizeText)
        {
            m_InstructionText.text = m_LocalizationManager.localizedObject;
        }
        else
        {
            m_InstructionText.text = k_FindObjectText;
        }
        m_FadeOn = true;
    }

    public void ShowFindFace()
    {
        m_VideoPlayer.clip = m_FindFaceClip;
        m_VideoPlayer.Play();
        if (m_LocalizeText)
        {
            m_InstructionText.text = m_LocalizationManager.localizedFace;
        }
        else
        {
            m_InstructionText.text = k_FindAFaceText;
        }
        m_FadeOn = true;
    }

    public void ShowCrossPlatformFindAPlane()
    {
        m_VideoPlayer.clip = m_FindAPlaneClip;
        m_VideoPlayer.Play();
        if (m_LocalizeText)
        {
            m_InstructionText.text = m_LocalizationManager.localizedMoveDevice;
        }
        else
        {
            m_InstructionText.text = k_MoveDeviceText;
        }
        m_FadeOn = true;
    }

    public void ShowCoachingOverlay()
    {
        if (m_CoachingOverlay)
        {
            if (m_CoachingOverlay.supported)
            {
                m_CoachingOverlay.ActivateCoaching(true);
                m_VideoPlayer.Stop();
                m_UsingARKitCoaching = true;
            }
            else
            {
                Debug.LogWarning("Coaching Overlay not supported on this platform");
            }
        }
    }

    public bool ARKitCoachingOverlaySupported()
    {
        if (m_CoachingOverlay)
        {
            return m_CoachingOverlay.supported;
        }

        return false;
    }
    
    public void FadeOffCurrentUI()
    {
        // assumes coaching overlay is first in the order
        if (m_UsingARKitCoaching)
        {
            // disables it instantly rather than animating it off
            m_CoachingOverlay.DisableCoaching(false);
            m_UsingARKitCoaching = false;
            m_InstructionText.color = m_AlphaWhite;
            if (onFadeOffComplete != null)
            {
                onFadeOffComplete();
            }
            m_FadeOff = true;
        }
        
        if (m_VideoPlayer.clip != null)
        {
            // handle exiting fade out early if currently fading out another Clip
            if (m_Tweening || m_FadeOn)
            {
                // stop tween immediately
                m_TweenTime = 1.0f;
                m_RawImage.color = m_AlphaWhite;
                m_InstructionText.color = m_AlphaWhite;
                if (onFadeOffComplete != null)
                {
                    onFadeOffComplete();
                }
            }

            m_FadeOff = true;
        }
    }
}
