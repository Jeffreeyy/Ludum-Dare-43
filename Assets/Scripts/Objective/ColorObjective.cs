using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjective : MonoBehaviour, ICollidable
{
    [SerializeField] private ColorItem m_Data;
    private Renderer m_Renderer;

    public CollidableType Type { get; set; }

    private void Start()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
        Type = CollidableType.Objective;
        UpdateColor();
    }

    private void UpdateColor()
    {
        m_Renderer.material = m_Data.material;
    }

    public void OnHit()
    {

    }

    public Colors GetColor()
    {
        return m_Data.color;
    }
}
