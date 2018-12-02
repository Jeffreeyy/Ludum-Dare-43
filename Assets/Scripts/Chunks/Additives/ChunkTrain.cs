using UnityEngine;
using DG.Tweening;

public class ChunkTrain : ChunkAdditive
{
    [SerializeField] private Transform m_Train;
    [SerializeField] private Vector2 m_StartingPoint, m_EndPoint;
    [SerializeField] private float m_MoveTime;
    public override void Show()
    {
        ResetPosition();
        m_Train.DOMoveX(m_EndPoint.x, m_MoveTime).SetEase(Ease.Linear);
    }

    public override void Hide()
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        m_Train.DOKill();
        m_Train.transform.localPosition = m_StartingPoint;
    }
}