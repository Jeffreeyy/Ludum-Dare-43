using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickup : MonoBehaviour, ICollidable
{

    private ColorItem m_Data;
    [SerializeField] private Renderer m_Renderer;

    public CollidableType Type { get; set; }
    public Colors Color { get { return m_Data.color; } }

    public void SetData(ColorItem data, CollidableType type)
    {
        m_Data = data;
        Type = type;

        UpdateColor();
    }

    private void UpdateColor()
    {
        m_Renderer.material = m_Data.material;
    }

    public void OnHit()
    {
        m_Renderer.enabled = false;//material = ColorLibrary.Instance.GetMaterial(Colors.White);
    }
}
