using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointHandler : MonoBehaviour
{

    [SerializeField]
    Transform m_Root;

    [SerializeField]
    Transform m_RootParent;

    public List<Transform> Joints { get; set; }

    void Start()
    {
        GetJoints();
    }

    public void GetJoints()
    {
        Joints = new List<Transform>();

        Queue<Transform> nodes = new Queue<Transform>();
        nodes.Enqueue(m_Root);
        while (nodes.Count > 0)
        {
            Transform next = nodes.Dequeue();
            for (int i = 0; i < next.childCount; ++i)
            {
                nodes.Enqueue(next.GetChild(i));
            }
            
            Joints.Add(next);
        }
        
        List<Transform> m_NewJoints = Joints;
        Joints = new List<Transform>();

        Joints.Add(m_RootParent);
        for (int i = 0; i < m_NewJoints.Count; i++)
        {
            Joints.Add(m_NewJoints[i]);
        }
        Debug.Log("Got all joints");
    }
    
}
