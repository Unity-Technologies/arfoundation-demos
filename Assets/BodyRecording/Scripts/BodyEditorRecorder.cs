using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class BodyEditorRecorder : MonoBehaviour
{
    [SerializeField]
    AnimationClip m_Clip;
    bool m_Record = false;
    BodyPlayback m_BodyPlayback;
    
    

#if UNITY_EDITOR
    GameObjectRecorder m_Recorder;

    void Start()
    {
        m_BodyPlayback = GetComponent<BodyPlayback>();
        m_Recorder = new GameObjectRecorder(gameObject);

        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    void LateUpdate()
    {
        if (m_Clip == null)
            return;

        if (m_Record)
        {
            m_Recorder.TakeSnapshot(1.0f/60.0f);
        }
        else if (m_Recorder.isRecording)
        {
            m_Recorder.SaveToClip(m_Clip);
            m_Recorder.ResetRecording();
            Debug.Log("Animation Capture Complete");
        }
    }
#endif
    
    public void RecordToggle()
    {
        m_Record = !m_Record;
    }
}
