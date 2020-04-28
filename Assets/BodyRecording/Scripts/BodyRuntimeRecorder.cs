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

    [SerializeField]
    GameObject m_RecordingIndicator;

    bool m_IsRecording = false;
    bool m_FirstRotation = false;
    Color[] m_RandomColors = new[] { s_LightBlue, s_LightGreen, s_LightRed };
    int m_CurrentJoint = 0;
    Pose m_BodyAnchorPose;
    Quaternion m_AlightmentRotation;
    ARHumanBodyManager m_HumanBodyManager;
    JointHandler m_JointHandler;
    
    List<Vector3> m_JointPositions;
    List<Quaternion> m_JointRotations;

    public List<Vector3> JointPositions => m_JointPositions;
    public List<Quaternion> JointRotations => m_JointRotations;
    
    static Color s_LightBlue = new Color(0.66f, 0.80f, 0.94f, 1.0f);
    static Color s_LightGreen = new Color(0.74f, 0.87f, 0.74f, 1.0f);
    static Color s_LightRed = new Color(0.87f, 0.74f, 0.74f, 1.0f);
    
    public static BodyRuntimeRecorder Instance;

    public event Action dataRecorded;

    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        int colorIndex = Random.Range(0, m_RandomColors.Length);
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].color = m_RandomColors[colorIndex];
        }

        m_JointHandler = GetComponent<JointHandler>();
        m_JointHandler.GetJoints();
        m_HumanBodyManager = FindObjectOfType<ARHumanBodyManager>();
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
        }

        if (bodyChangeEventArgs.updated.Count > 0)
        {
            m_BodyAnchorPose = bodyChangeEventArgs.updated[0].pose;
        }
        
        // TODO handle no longer tracking
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        
        m_JointPositions = new List<Vector3>();
        m_JointRotations = new List<Quaternion>();
    }

    void Update()
    {
        if (m_IsRecording)
        {
            m_JointPositions.Add(m_BodyAnchorPose.position);
            m_JointRotations.Add(m_BodyAnchorPose.rotation);
            
            // adjustment for recording anchor pose as 0th joint
            for (int i = 1; i < m_JointHandler.Joints.Count; i++)
            {
                m_JointPositions.Add(m_JointHandler.Joints[i].localPosition); 
                m_JointRotations.Add(m_JointHandler.Joints[i].localRotation);
            }
        }
        
        m_RecordingIndicator.SetActive(m_IsRecording);
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




