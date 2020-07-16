using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ProbePlacement : MonoBehaviour
{
    [SerializeField]
    AREnvironmentProbeManager m_ProbeManager;

    [SerializeField]
    ARRaycastManager m_RaycastManager;

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
                    m_ProbeManager.AddEnvironmentProbe(hitPose, Vector3.one, Vector3.one);
                }
            }
        }
    }
}
