using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessCapture : MonoBehaviour
{

    List<Vector3> m_PositionValues;
    List<Quaternion> m_RotationValues;
    List<Transform> m_Joints;

    bool m_FileProcessed = false;
    bool m_AnimationComplete = false;
    bool m_StartingRecord = false;
    int m_LineCount = 0;

    [SerializeField]
    Transform m_BodyRoot;

    [SerializeField]
    Transform m_BodyRootParent;

    [SerializeField]
    BodyEditorRecorder m_BodyEditorRecorder;

    [SerializeField]
    bool m_ProcessPosition = false;

    [SerializeField]
    bool m_ProcessRotation = true;

    [SerializeField]
    string m_PostFilePath;

    [SerializeField]
    GameObject m_OnionSkinPrefab;
    
    static int s_TargetFramerate = 60;
    static string s_FilePath = "/DanWork/Captures/RecordedTransform";

    int m_FrameCount = 0;

    Vector3 m_LerpPosition;
    Quaternion m_LerpRotation;
    Vector3 m_RootPosition;
    bool m_ShowingOnionSkinning;
    List<GameObject> m_OnionObjects;
    

    void Start ()
	{
	    Application.targetFrameRate = s_TargetFramerate;
        m_PositionValues = new List<Vector3>();
        m_RotationValues = new List<Quaternion>();
        m_Joints = new List<Transform>();

        Queue<Transform> nodes = new Queue<Transform>();
        nodes.Enqueue(m_BodyRoot);
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

        m_Joints.Add(m_BodyRootParent);
        for (int i = 0; i < m_NewJoints.Count; i++)
        {
            m_Joints.Add(m_NewJoints[i]);
        }
        
        Debug.Log("Joints Found: "+m_Joints.Count);
        
	    ProcessFile();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_FileProcessed)
        {
            if (!m_StartingRecord)
            {
                m_StartingRecord = true;
                m_BodyEditorRecorder.RecordToggle();
            }
            if (!m_AnimationComplete)
            {
                for (int i = 0; i < m_Joints.Count; i++)
                {
                    if (i == 0)
                    {
                        Vector3 m_JointPositionWorld = m_PositionValues[m_LineCount] + // recording point from capture
                                new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) - // adjusted position based on parent root / transform which is moveable
                                m_PositionValues[0]; // first recorded hip position, subtracted to "zero out" captured translation
                        
                        m_BodyRootParent.localPosition = m_JointPositionWorld;
                        m_BodyRootParent.localRotation = m_RotationValues[m_LineCount];
                        
                    }
                    else
                    {
                        if (m_ProcessPosition)
                        {
                            m_Joints[i].transform.localPosition = m_PositionValues[m_LineCount];
                        }

                        if (m_ProcessRotation)
                        {
                            m_Joints[i].transform.localRotation = m_RotationValues[m_LineCount];
                        }
                    }

                    m_LineCount++;
                }

                // read all lines of the file
                if (m_LineCount == m_PositionValues.Count)
                {
                    m_AnimationComplete = true;
                    m_BodyEditorRecorder.RecordToggle();
                }
            }
        }

        m_FrameCount++;

    }

    void ProcessFile()
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath + s_FilePath+m_PostFilePath+".txt"); //load text file with data
        while ((line = file.ReadLine()) != null)
        { //while text exists.. repeat

            char[] delimiterChar = { ')' };//variable separation
            string[] split = line.Split(delimiterChar, StringSplitOptions.None); //split vector3 and quat into split[0] and split[1]

            // remove first ( char and ,( for quat
            split[0] = split[0].Remove(0, 1);
            split[1] = split[1].Remove(0, 2);

            string[] vecSplit = split[0].Split(','); // split up vector3 into just numbers, 
            string[] quatSplit = split[1].Split(','); // split up quat into just numbers
            
            Vector3 newPOS = new Vector3(float.Parse(vecSplit[0]), float.Parse(vecSplit[1]), float.Parse(vecSplit[2]));
            Quaternion newROT = new Quaternion(float.Parse(quatSplit[0]), float.Parse(quatSplit[1]), float.Parse(quatSplit[2]), float.Parse(quatSplit[3]));

            m_PositionValues.Add(newPOS);
            m_RotationValues.Add(newROT);
        }
        file.Close();

        m_RootPosition = m_PositionValues[0];

        m_FileProcessed = true;
    }

    public void ShowOnionSkin()
    {
        int displayIndex = 0;
        bool setRoot = false;
        if (!m_ShowingOnionSkinning)
        {
            GameObject posedRobot = Instantiate(m_OnionSkinPrefab, Vector3.zero, Quaternion.identity);
            posedRobot.transform.parent = transform;
            posedRobot.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            if (!setRoot)
            {
                Vector3 m_JointPositionWorld = m_PositionValues[m_LineCount] + // recording point from capture
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) - // adjusted position based on parent root / transform which is moveable
                    m_PositionValues[0]; // first recorded hip position, subtracted to "zero out" captured translation
                        
                m_BodyRootParent.localPosition = m_JointPositionWorld;
                m_BodyRootParent.localRotation = m_RotationValues[m_LineCount];

                // todo set alignment rotation
                posedRobot.GetComponent<BodyPoser>().SetJointPositionAndRotation(m_JointPositionWorld, m_RotationValues[m_LineCount]);
                setRoot = true;
            }

            m_ShowingOnionSkinning = true;
        }
    }
}
