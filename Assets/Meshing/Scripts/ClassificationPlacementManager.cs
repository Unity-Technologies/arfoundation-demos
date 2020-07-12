using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARKit;

public class ClassificationPlacementManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_FloorPrefabs;

    [SerializeField]
    List<GameObject> m_TablePrefabs;

    [SerializeField]
    List<GameObject> m_WallPrefabs;

    [SerializeField]
    MeshClassificationManager m_ClassificationManager;

    [SerializeField]
    PlacementReticle m_Reticle;

    Touch m_Touch;
    List<RaycastResult> m_OverUIResults = new List<RaycastResult>();

    [SerializeField]
    GameObject m_FloorUI;

    [SerializeField]
    GameObject m_WallUI;

    [SerializeField]
    GameObject m_TableUI;

    bool m_ShowingSelectionUI;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            m_Touch = Input.GetTouch(0);

            // touched UI, return early
            if (IsTouchOverUIObject(m_Touch))
            {
                return;
            }

            
            // valid surfaces for objects
            if (m_ClassificationManager.currentClassification == ARMeshClassification.Floor)
            {
                m_FloorUI.SetActive(true);
                m_ShowingSelectionUI = true;
            }

            else if (m_ClassificationManager.currentClassification == ARMeshClassification.Wall)
            {
                m_WallUI.SetActive(true);
                m_ShowingSelectionUI = true;
            }
            
            else if(m_ClassificationManager.currentClassification == ARMeshClassification.Table)
            {
                m_TableUI.SetActive(true);    
                m_ShowingSelectionUI = true;

            }
            else
            {
                m_FloorUI.SetActive(false);
                m_WallUI.SetActive(false);
                m_TableUI.SetActive(false);
                m_ShowingSelectionUI = false;
            }
        }
    }

    public void PlaceFloorObject(int indexToPlace)
    {
        Instantiate(m_FloorPrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        m_FloorUI.SetActive(false);
        m_ShowingSelectionUI = false;
    }

    public void PlaceWallObject(int indexToPlace)
    {
        Instantiate(m_WallPrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        m_WallUI.SetActive(false);
        m_ShowingSelectionUI = false;
    }
    
    public void PlaceTableObject(int indexToPlace)
    {
        Instantiate(m_TablePrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        m_TableUI.SetActive(false);
        m_ShowingSelectionUI = false;
    }

    bool IsTouchOverUIObject(Touch touch)
    {   
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touch.position;
        m_OverUIResults.Clear();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, m_OverUIResults);
        return m_OverUIResults.Count > 0;
    }
    
}
