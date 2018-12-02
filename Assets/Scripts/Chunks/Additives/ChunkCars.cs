using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public class ChunkCar
{
    public Transform car;
    public Vector2 startingPoint, endPoint;
    public float moveTime;
}

public class ChunkCars : ChunkAdditive
{
    [SerializeField] private List<ChunkCar> m_Cars = new List<ChunkCar>();
    public override void Show()
    {
        ResetPosition();
        for (int i = 0; i < m_Cars.Count; i++)
            m_Cars[i].car.DOMoveX(m_Cars[i].endPoint.x, m_Cars[i].moveTime).SetEase(Ease.Linear);
    }

    public override void Hide()
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        for (int i = 0; i < m_Cars.Count; i++)
        {
            m_Cars[i].car.DOKill();
            m_Cars[i].car.transform.localPosition = new Vector3(m_Cars[i].startingPoint.x, 0, m_Cars[i].startingPoint.y);
        }
    }
}