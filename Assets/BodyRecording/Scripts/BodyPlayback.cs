using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodyPlayback : MonoBehaviour
{
    [SerializeField]
    bool m_InEditor;

    public bool inEditor
    {
        get => m_InEditor;
        set => m_InEditor = value;
    }

    [SerializeField]
    bool m_PlayingAnimation = false;
    
    JointHandler m_JointHandler;
    BodyRuntimeRecorder m_BodyRuntimeRecorder;
    
    
    int m_JointIndex = 0;
    bool m_PlayingOneShot = false;

    List<Vector3> m_JointPositions;
    List<Quaternion> m_JointRotations;
    
    void OnEnable()
    {
        m_JointHandler = GetComponent<JointHandler>();
        m_JointHandler.GetJoints();

        m_BodyRuntimeRecorder = FindObjectOfType<BodyRuntimeRecorder>();

        // AR Playback
        if (!m_InEditor)
        {
            m_JointPositions = m_BodyRuntimeRecorder.JointPositions;
            m_JointRotations = m_BodyRuntimeRecorder.JointRotations;
        }
        // Editor processing, file playback
        else
        {
            BodyFileReader reader = GetComponent<BodyFileReader>();
            reader.ProcessFile();
            m_JointPositions = reader.positionValues;
            m_JointRotations = reader.rotationValues;
        }
        
        SetBodyStartPose();
    }

    void Update()
    {
        LoopPlayback();
    }

    void LoopPlayback()
    {
        if (m_PlayingAnimation)
        {
            for (int i = 0; i < m_JointHandler.Joints.Count; i++)
            {
                if (i == 0)
                {
                    // hip joint position adjustment to center around placed object
                    Vector3 m_JointPositionWorld = m_JointPositions[m_JointIndex] - m_JointPositions[0];

                    m_JointHandler.Joints[0].transform.localPosition = m_JointPositionWorld;
                    m_JointHandler.Joints[0].transform.localRotation = m_JointRotations[m_JointIndex];
                }
                else
                {
                    m_JointHandler.Joints[i].transform.localPosition = m_JointPositions[m_JointIndex];
                    m_JointHandler.Joints[i].transform.localRotation = m_JointRotations[m_JointIndex];
                }
                
                
                m_JointIndex++;
            }

            // resets animation
            if (m_JointIndex == m_JointPositions.Count)
            {
                m_JointIndex = 0;
                if (m_InEditor && m_PlayingOneShot)
                {
                    m_PlayingAnimation = false;
                    GetComponent<BodyEditorRecorder>().RecordToggle();
                }
            }
        }
    }
    
    void SetBodyStartPose()
    {
        for (int i = 0; i < m_JointHandler.Joints.Count; i++)
        {
            m_JointHandler.Joints[i].transform.localRotation = m_JointRotations[i];
        }
    }

    public void AnimationToggle()
    {
        m_PlayingAnimation = !m_PlayingAnimation;
    }

    Vector3 CalculateAveragePosition(int rootIndex)
    {
        int numberOfCapturedPositions = m_JointPositions.Count / 92;
        Vector3 retVal = Vector3.zero;
        
        for (int i = 0; i < numberOfCapturedPositions; i++)
        {
            retVal += m_JointPositions[i * 92];
        }

        return retVal / numberOfCapturedPositions;
    }

    public void PlayAnimationOneShot()
    {
        m_PlayingAnimation = true;
        m_PlayingOneShot = true;
    }
}
