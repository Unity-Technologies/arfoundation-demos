using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationManager : MonoBehaviour
{
    public enum SupportedLanguages
    {
        ChineseSimplified,
        English,
        French,
        German,
        Hindi,
        Italian,
        Japanese,
        Korean,
        Portuguese,
        Russian,
        Spanish,
        Tamil,
        Telugu
    }

    public SupportedLanguages CurrentLocalizedLanguage;

    const string k_ReasonTable = "Reasons";
    const string k_UXTable = "UX";
    const string k_InitializeKey = "INIT";
    const string k_MotionKey = "MOTION";
    const string k_LightKey = "LIGHT";
    const string k_FeaturesKey = "FEATURES";
    const string k_UnsupportedKey = "UNSUPPORTED";
    const string k_NoneKey = "NONE";
    const string k_MoveDeviceKey = "MOVE_DEVICE";
    const string k_TapToPlaceKey = "TAP_TO_PLACE";
    const string k_BodyKey = "FIND_BODY";
    const string k_FaceKey = "FIND_FACE";
    const string k_ImageKey = "FIND_IMAGE";
    const string k_ObjectKey = "FIND_OBJECT";

    public string localizedInit;
    public string localizedMotion;
    public string localizedLight;
    public string localizedFeatures;
    public string localizedUnsupported;
    public string localizedNone;
    public string localizedMoveDevice;
    public string localizedTapToPlace;
    public string localizedBody;
    public string localizedFace;
    public string localizedImage;
    public string localizedObject;
    
    bool m_ReasonsComplete = false;
    bool m_UXComplete = false;

    public bool localizationComplete => m_ReasonsComplete && m_UXComplete;

    [SerializeField]
    TMP_FontAsset m_SimplifiedChineseFont;

    public TMP_FontAsset simplifiedChineseFont
    {
        get => m_SimplifiedChineseFont;
        set => m_SimplifiedChineseFont = value;
    }

    [SerializeField]
    TMP_FontAsset m_JapaneseFont;

    public TMP_FontAsset japaneseFont
    {
        get => m_JapaneseFont;
        set => m_JapaneseFont = value;
    }

    [SerializeField]
    TMP_FontAsset m_KoreanFont;
    
    public TMP_FontAsset koreanFont
    {
        get => m_KoreanFont;
        set => m_KoreanFont = value;
    }

    [SerializeField]
    TMP_FontAsset m_TamilFont;

    public TMP_FontAsset tamilFont
    {
        get => m_TamilFont;
        set => m_TamilFont = value;
    }

    [SerializeField]
    TMP_FontAsset m_HindiFont;
    
    public TMP_FontAsset hindiFont
    {
        get => m_HindiFont;
        set => m_HindiFont = value;
    }

    [SerializeField]
    TMP_FontAsset m_TeluguFont;

    public TMP_FontAsset teluguFont
    {
        get => m_TeluguFont;
        set => m_TeluguFont = value;
    }

    [SerializeField]
    TMP_Text m_InstructionText;

    public TMP_Text instructionText
    {
        get => m_InstructionText;
        set => m_InstructionText = value;
    }

    [SerializeField]
    TMP_Text m_ReasonText;

    public TMP_Text reasonText
    {
        get => m_ReasonText;
        set => m_ReasonText = value;
    }
    
    const int k_MaxAutoSizeSC = 70;

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        
        LocalizationSettings.AvailableLocales.Locales.Sort();
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)CurrentLocalizedLanguage];
        SwapFonts(CurrentLocalizedLanguage);
        
        LocalizationSettings.StringDatabase.GetTableAsync(k_ReasonTable).Completed += OnCompletedReasons;
        LocalizationSettings.StringDatabase.GetTableAsync(k_UXTable).Completed += OnCompletedUX;
    }

    void OnCompletedUX(AsyncOperationHandle<StringTable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            var uxTable = obj.Result;
            localizedMoveDevice = uxTable.GetEntry(k_MoveDeviceKey).GetLocalizedString();
            localizedTapToPlace = uxTable.GetEntry(k_TapToPlaceKey).GetLocalizedString();
            localizedBody = uxTable.GetEntry(k_BodyKey).GetLocalizedString();
            localizedFace = uxTable.GetEntry(k_FaceKey).GetLocalizedString();
            localizedImage = uxTable.GetEntry(k_ImageKey).GetLocalizedString();
            localizedObject = uxTable.GetEntry(k_ObjectKey).GetLocalizedString();

            m_UXComplete = true;
        }
    }

    void OnCompletedReasons(AsyncOperationHandle<StringTable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            var reasonsTable = obj.Result;
            localizedInit = reasonsTable.GetEntry(k_InitializeKey).GetLocalizedString();
            localizedMotion = reasonsTable.GetEntry(k_MotionKey).GetLocalizedString();
            localizedLight = reasonsTable.GetEntry(k_LightKey).GetLocalizedString();
            localizedFeatures = reasonsTable.GetEntry(k_FeaturesKey).GetLocalizedString();
            localizedUnsupported = reasonsTable.GetEntry(k_UnsupportedKey).GetLocalizedString();
            localizedNone = reasonsTable.GetEntry(k_NoneKey).GetLocalizedString();

            m_ReasonsComplete = true;
        }
    }
    
    void SwapFonts(SupportedLanguages selectedLanguage)
    {
        TMP_FontAsset m_FontToSet = null;
        // swap fonts for Simplified Chinese, Japanese, Korean, Tamil, Hindi and Telugu
        switch (selectedLanguage)
        {
            case SupportedLanguages.ChineseSimplified:
                m_FontToSet = m_SimplifiedChineseFont;
                // custom size adjustment for legibility
                m_InstructionText.fontSizeMax = k_MaxAutoSizeSC;
                m_ReasonText.fontSizeMax = k_MaxAutoSizeSC;
                break;
            case SupportedLanguages.Japanese:
                m_FontToSet = m_JapaneseFont;
                break;
            case SupportedLanguages.Korean:
                m_FontToSet = m_KoreanFont;
                break;
            case SupportedLanguages.Tamil:
                m_FontToSet = m_TamilFont;
                break;
            case SupportedLanguages.Hindi:
                m_FontToSet = m_HindiFont;
                break;
            case SupportedLanguages.Telugu:
                m_FontToSet = m_TeluguFont;
                break;
        }

        if (m_FontToSet != null)
        {
            m_InstructionText.font = m_FontToSet;
            m_ReasonText.font = m_FontToSet;
        }
    }
}
