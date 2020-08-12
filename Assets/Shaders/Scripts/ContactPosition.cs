using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactPosition : MonoBehaviour
{
    private Material m_TargetMaterial;
    private int m_PropertyHash;

    public GameObject m_TargetObject;

    // Start is called before the first frame update
    void Start()
    {
        m_PropertyHash = Shader.PropertyToID("_ContactPosition");
        m_TargetMaterial = m_TargetObject.GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        m_TargetMaterial.SetVector(m_PropertyHash, transform.position);   
    }
}
