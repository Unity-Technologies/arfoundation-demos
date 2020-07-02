using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ProbePlacement : MonoBehaviour
{
    [SerializeField]
    AREnvironmentProbeManager m_ProbeManager;

    [SerializeField]
    Camera m_Camera;

    RaycastHit m_Hit;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(m_Camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out m_Hit))
                {
                    Pose hitPose = new Pose(m_Hit.point, Quaternion.identity);
                    Vector3 m_Five = new Vector3(5, 5,5);
                    m_ProbeManager.AddEnvironmentProbe(hitPose, m_Five, m_Five);
                }
            }
        }
    }
}
