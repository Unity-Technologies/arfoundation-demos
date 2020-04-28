using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPoser : MonoBehaviour
{
    [SerializeField]
    Transform m_Root;

    [SerializeField]
    Transform m_RootParent;

    List<Transform> m_Joints = new List<Transform>();
    int m_JointIndex = 0;
    int m_TotalJoints = 92;

    void GetJoints()
    {
        Queue<Transform> nodes = new Queue<Transform>();
        nodes.Enqueue(m_Root);
        while (nodes.Count > 0)
        {
            Transform next = nodes.Dequeue();
            for (int i = 0; i < next.childCount; ++i)
            {
                nodes.Enqueue(next.GetChild(i));
            }
            
            m_Joints.Add(next);
        }

        List<Transform> m_NewJoints = m_Joints;
        m_Joints = new List<Transform>();

        m_Joints.Add(m_RootParent);
        for (int i = 0; i < m_NewJoints.Count; i++)
        {
            m_Joints.Add(m_NewJoints[i]);
        }
    }

    public void SetJointPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        m_Joints[m_JointIndex].SetPositionAndRotation(pos, rot);
        m_JointIndex++;
    }
}
