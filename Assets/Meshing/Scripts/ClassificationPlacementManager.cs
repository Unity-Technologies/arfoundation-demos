using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    
}
