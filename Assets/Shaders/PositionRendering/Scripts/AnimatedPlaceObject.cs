using DG.Tweening;
using UnityEngine;

public class AnimatedPlaceObject : MonoBehaviour
{
    [SerializeField]
    Transform m_PlacementSphere;

    public Transform placementSphere
    {
        get => m_PlacementSphere;
        set => m_PlacementSphere = value;
    }
    
    const float k_ShakeScale = 0.5f;
    const Ease k_MoveEase = Ease.OutBounce;

    public void AnimatePlacement()
    {
        m_PlacementSphere.DOLocalMove(Vector3.zero, 0.1f).SetEase(k_MoveEase).OnComplete(BounceLand);
    }

    void BounceLand()
    {
        m_PlacementSphere.DOShakeScale(0.1f, k_ShakeScale);
    }
}
