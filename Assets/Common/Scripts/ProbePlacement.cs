using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ProbePlacement : MonoBehaviour
{
    [SerializeField]
    AREnvironmentProbeManager m_ProbeManager;

    public AREnvironmentProbeManager probeManager
    {
        get => m_ProbeManager;
        set => m_ProbeManager = value;
    }

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    public ARRaycastManager raycastManager
    {
        get => m_RaycastManager;
        set => m_RaycastManager = value;
    }

    List<ARRaycastHit> k_Hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (m_RaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), k_Hits))
                {
                    Pose hitPose = k_Hits[0].pose;
                    GameObject m_ProbeObject = new GameObject("EnvProbeObject");
                    m_ProbeObject.transform.position = hitPose.position;
                    m_ProbeObject.AddComponent<AREnvironmentProbe>();
                }
            }
        }
    }
}
