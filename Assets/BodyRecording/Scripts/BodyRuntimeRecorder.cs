using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class BodyRuntimeRecorder : MonoBehaviour
{
    [SerializeField]
    Material[] m_Materials;

    bool m_IsRecording = false;
    bool m_FirstRotation = false;
    int m_CurrentJoint = 0;
    Pose m_BodyAnchorPose;
    Quaternion m_AlightmentRotation;
    
    
    List<Vector3> m_JointPositions;
    List<Quaternion> m_JointRotations;

    public List<Vector3> JointPositions => m_JointPositions;
    public List<Quaternion> JointRotations => m_JointRotations;
    
    
    public event Action dataRecorded;


    [SerializeField]
    ARHumanBodyManager m_HumanBodyManager;

    JointHandler m_ActiveTrackedBodyJoints;

    static int s_TrackedBodyJointCount = 92; // 91 + root parent
    
    JointHandler m_JointHandler;

















    void OnEnable()
    {
        m_HumanBodyManager.humanBodiesChanged += HumanBodyManagerOnhumanBodiesChanged;
    }

    void OnDisable()
    {
        m_HumanBodyManager.humanBodiesChanged -= HumanBodyManagerOnhumanBodiesChanged;
    }

    void HumanBodyManagerOnhumanBodiesChanged(ARHumanBodiesChangedEventArgs bodyChangeEventArgs)
    {
        if (bodyChangeEventArgs.added.Count > 0)
        {
            m_BodyAnchorPose = bodyChangeEventArgs.added[0].pose;
            m_ActiveTrackedBodyJoints = bodyChangeEventArgs.added[0].transform.GetChild(0).GetComponent<JointHandler>();
        }

        if (bodyChangeEventArgs.updated.Count > 0)
        {
            m_BodyAnchorPose = bodyChangeEventArgs.updated[0].pose;
            if (m_ActiveTrackedBodyJoints == null)
            {
                m_ActiveTrackedBodyJoints = bodyChangeEventArgs.updated[0].transform.GetChild(0).GetComponent<JointHandler>();
            }
        }
        
        // TODO handle no longer tracking
    }

    void Start()
    {
        m_JointPositions = new List<Vector3>();
        m_JointRotations = new List<Quaternion>();
    }

    void Update()
    {
        if (m_IsRecording)
        {
            for (int i = 0; i < s_TrackedBodyJointCount; i++)
            {
                if (i == 0)
                {
                    // store root parent / anchor in world space
                    m_JointPositions.Add(m_ActiveTrackedBodyJoints.Joints[i].position);
                    m_JointRotations.Add(m_ActiveTrackedBodyJoints.Joints[i].rotation);
                }
                else
                {
                    // store rig joints in local space
                    m_JointPositions.Add(m_ActiveTrackedBodyJoints.Joints[i].localPosition);
                    m_JointRotations.Add(m_ActiveTrackedBodyJoints.Joints[i].localRotation);
                }
            }
            
            
            /*
            // store anchor pose world position and rotation
            m_JointPositions.Add(m_BodyAnchorPose.position);
            m_JointRotations.Add(m_BodyAnchorPose.rotation);
            
            // offset by one to start storing local joints 
            for (int i = 1; i < m_JointHandler.Joints.Count; i++)
            {
                m_JointPositions.Add(m_JointHandler.Joints[i].localPosition); 
                m_JointRotations.Add(m_JointHandler.Joints[i].localRotation);
            }
            */
        }
        
        //m_RecordingIndicator.SetActive(m_IsRecording);
    }

    public void RecordingToggle()
    {
        m_IsRecording = !m_IsRecording;

        if (!m_IsRecording && m_JointPositions.Count > 0)
        {
            if (dataRecorded != null)
            {
                dataRecorded();
            }
        }
    }

    public bool HasData()
    {
        return m_JointPositions != null && m_JointPositions.Count > 0;
    }
    
}




