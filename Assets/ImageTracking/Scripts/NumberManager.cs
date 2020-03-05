using System;
using UnityEngine;
using UnityEngine.Animations;

public class NumberManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Number Object in the prefab")]
    GameObject m_NumberObject;

    public GameObject numberObject
    {
        get => m_NumberObject;
        set => m_NumberObject = value;
    }

    AimConstraint m_Constraint;

    void OnEnable()
    {
        ConstraintSource m_Source = new ConstraintSource();
        m_Source.sourceTransform = Camera.main.transform;
        m_Source.weight = 1.0f;
        m_Constraint = GetComponentInChildren<AimConstraint>();
        m_Constraint.AddSource(m_Source);
    }

    public void Enable3DNumber(bool enable)
    {
        m_NumberObject.SetActive(enable);
    }
}
