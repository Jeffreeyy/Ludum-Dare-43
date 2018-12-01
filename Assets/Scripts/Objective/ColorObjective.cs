using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjective : MonoBehaviour, ICollidable
{
    private ColorItem m_Data;
    private Renderer m_Renderer;

    public CollidableType Type { get; set; }

    public Colors Color { get { return m_Data.color; } }

    public void SetData(ColorItem data, CollidableType type)
    {
        m_Data = data;
        Type = type;

        UpdateColor();
    }

    private void Awake()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
    }

    private void UpdateColor()
    {
        m_Renderer.material = m_Data.material;
    }

    public void OnHit()
    {
        // fireworks ?? idk

    }
}
