using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class AnimatedPlaceObject : MonoBehaviour
{

    [SerializeField]
    Transform m_PlacementSphere;

    public float ShakeVal;

    public Ease MoveEase;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_PlacementSphere.localPosition = new Vector3(0, 0.375f, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimatePlacement();
        }
#endif
    }

    public void AnimatePlacement()
    {
        m_PlacementSphere.DOLocalMove(Vector3.zero, 0.1f).SetEase(MoveEase).OnComplete(BounceLand);
    }

    void BounceLand()
    {
        m_PlacementSphere.DOShakeScale(0.1f, ShakeVal);
    }
}
