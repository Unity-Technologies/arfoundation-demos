using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactPositionPlacement : MonoBehaviour
{
    [SerializeField]
    ARContactPosition m_ARContactPosition;
    
    public ARContactPosition arContactPosition
    {
        get => m_ARContactPosition;
        set => m_ARContactPosition = value;
    }

    [SerializeField]
    GameObject m_ObjectPrefab;

    public GameObject objectPrefab
    {
        get => m_ObjectPrefab;
        set => m_ObjectPrefab = value;
    }

    GameObject m_SpawnedObject;
    bool m_Placed;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (m_SpawnedObject && !m_Placed)
                {
                    m_Placed = true;
                    
                    if(m_SpawnedObject.TryGetComponent(out AnimatedPlaceObject m_PlacedObject))
                    {
                        m_PlacedObject.AnimatePlacement();
                    }
                }
            }
        }

        if (m_SpawnedObject && !m_Placed)
        {
            m_SpawnedObject.transform.SetPositionAndRotation(m_ARContactPosition.targetPosition, m_ARContactPosition.targetRotation);
        }
    }

    public void SpawnPlacementObject()
    {
        m_SpawnedObject = Instantiate(m_ObjectPrefab, m_ARContactPosition.targetPosition, m_ARContactPosition.targetRotation);
        m_Placed = false;
    }
}
