using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScreenHelper : MonoBehaviour
{
    public static CenterScreenHelper Instance;

    float m_MidPointWidth;
    float m_MidPointHeight;
    Vector2 m_CenterScreenVert;
    Vector2 m_CenterScreenHor;
    ScreenOrientation m_CurrentOrientation;
    float m_CurrentWidth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        m_CurrentOrientation = Screen.orientation;
        m_CurrentWidth = Screen.width;
        m_MidPointWidth = Screen.width / 2;
        m_MidPointHeight = Screen.height / 2;

        if (m_CurrentOrientation == ScreenOrientation.Landscape)
        {
            m_CenterScreenVert = new Vector2(m_MidPointHeight, m_MidPointWidth);
            m_CenterScreenHor  = new Vector2(m_MidPointWidth, m_MidPointHeight);
        }
        else
        {
            m_CenterScreenVert = new Vector2(m_MidPointWidth, m_MidPointHeight);
            m_CenterScreenHor = new Vector2(m_MidPointHeight, m_MidPointWidth);
        }
    }

    public Vector2 GetCenterScreen()
    {
        if (Screen.width != m_CurrentWidth)
        {
            m_CurrentWidth = Screen.width;
            m_CurrentOrientation = Screen.orientation;
        }

        return m_CurrentOrientation == ScreenOrientation.Landscape ? m_CenterScreenHor : m_CenterScreenVert;
    }
}
