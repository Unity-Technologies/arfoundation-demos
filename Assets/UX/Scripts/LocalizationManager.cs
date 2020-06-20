using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    bool m_LocalizationComplete = false;

    public bool localizationComplete => m_LocalizationComplete;

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        // sort list of available languages to match the enum ordering
        LocalizationSettings.AvailableLocales.Locales.Sort();

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)CurrentLocalizedLanguage];

        // get all values at start, dynamic localization (changing language at runtime) not supported with this structure
        var m_Init = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_ReasonTable, k_InitializeKey);
        yield return m_Init;
        if (m_Init.IsDone && m_Init.Status == AsyncOperationStatus.Succeeded)
        {
            localizedInit = m_Init.Result;
        }

        var m_Motion = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_ReasonTable, k_MotionKey);
        yield return m_Motion;
        if (m_Motion.IsDone && m_Motion.Status == AsyncOperationStatus.Succeeded)
        {
            localizedMotion = m_Motion.Result;
        }

        var m_Light = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_ReasonTable, k_LightKey);
        yield return m_Light;
        if (m_Light.IsDone && m_Light.Status == AsyncOperationStatus.Succeeded)
        {
            localizedLight = m_Light.Result;
        }

        var m_Features = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_ReasonTable, k_FeaturesKey);
        yield return m_Features;
        if (m_Features.IsDone && m_Features.Status == AsyncOperationStatus.Succeeded)
        {
            localizedFeatures = m_Features.Result;
        }

        var m_Unsupported = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_ReasonTable, k_UnsupportedKey);
        yield return m_Unsupported;
        if (m_Unsupported.IsDone && m_Unsupported.Status == AsyncOperationStatus.Succeeded)
        {
            localizedUnsupported = m_Unsupported.Result;
        }

        var m_None = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_ReasonTable, k_NoneKey);
        yield return m_None;
        if (m_None.IsDone && m_None.Status == AsyncOperationStatus.Succeeded)
        {
            localizedNone = m_None.Result;
        }

        var m_Move = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_UXTable, k_MoveDeviceKey);
        yield return m_Move;
        if (m_Move.IsDone && m_Move.Status == AsyncOperationStatus.Succeeded)
        {
            localizedMoveDevice = m_Move.Result;
        }

        var m_Tap = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_UXTable, k_TapToPlaceKey);
        yield return m_Tap;
        if (m_Tap.IsDone && m_Tap.Status == AsyncOperationStatus.Succeeded)
        {
            localizedTapToPlace = m_Tap.Result;
        }

        var m_Body = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_UXTable, k_BodyKey);
        yield return m_Body;
        if (m_Body.IsDone && m_Body.Status == AsyncOperationStatus.Succeeded)
        {
            localizedBody = m_Body.Result;
        }

        var m_Face = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_UXTable, k_FaceKey);
        yield return m_Face;
        if (m_Face.IsDone && m_Face.Status == AsyncOperationStatus.Succeeded)
        {
            localizedFace = m_Face.Result;
        }

        var m_Image = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_UXTable, k_ImageKey);
        yield return m_Image;
        if (m_Image.IsDone && m_Image.Status == AsyncOperationStatus.Succeeded)
        {
            localizedImage = m_Image.Result;
        }

        var m_Object = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(k_UXTable, k_ObjectKey);
        yield return m_Object;
        if (m_Object.IsDone && m_Object.Status == AsyncOperationStatus.Succeeded)
        {
            localizedObject = m_Object.Result;
        }

        m_LocalizationComplete = true;
    }
}
