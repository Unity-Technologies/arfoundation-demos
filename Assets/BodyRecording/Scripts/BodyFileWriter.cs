using System;
using System.IO;
using UnityEngine;

public class BodyFileWriter : MonoBehaviour
{
    string m_FilePath; 
    BodyRuntimeRecorder m_BodyRuntimeRecorder;

    static string s_TextFileName = "BodyCaptureData.txt";

    void OnEnable()
    {
        m_BodyRuntimeRecorder = GetComponent<BodyRuntimeRecorder>();
    }

    public void Share()
    {
        // write captured data to a text file
        string path = Application.persistentDataPath + "/" + s_TextFileName;
        StreamWriter writer = new StreamWriter(path, false);

        for (int i = 0; i < m_BodyRuntimeRecorder.JointPositions.Count; i++)
        {
            writer.WriteLine(m_BodyRuntimeRecorder.JointPositions[i].ToString("F7") + "," + m_BodyRuntimeRecorder.JointRotations[i].ToString("F7"));
        }
        writer.Close();

        // native share via native share plugin https://assetstore.unity.com/packages/tools/integration/native-share-for-android-ios-112731
        NativeShare m_nativeShare = new NativeShare();
        m_nativeShare.AddFile(Application.persistentDataPath + "/" + s_TextFileName);
        m_nativeShare.Share();
    }
}
