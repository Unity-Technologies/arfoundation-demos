using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFileReader : MonoBehaviour
{
    [SerializeField]
    string m_PostFilePath;

    BodyRuntimeRecorder m_BodyRuntimeRecorder;
    List<Vector3> m_PositionValues = new List<Vector3>();
    List<Quaternion> m_RotationValues = new List<Quaternion>();
    
    public List<Vector3> positionValues => m_PositionValues;
    public List<Quaternion> rotationValues => m_RotationValues;
    static string s_FilePath = "/DanWork/Captures/RecordedTransform";
    
    public void ProcessFile()
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
        Debug.Log("file processed");
    }
}
