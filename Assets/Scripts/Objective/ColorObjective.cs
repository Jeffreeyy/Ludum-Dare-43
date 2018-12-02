using UnityEngine;

public class ColorObjective : MonoBehaviour, ICollidable
{
    private ColorItem m_Data;
    private Renderer m_Renderer;

    public CollidableType Type { get; set; }

    public Colors Color { get { return m_Data.color; } }

    public Chunk ChunkParent { get; set; }

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
        Color color = m_Data.material.color;
        m_Renderer.material.color = new Color(color.r, color.g, color.b, 0.4f);
        m_Renderer.material.SetColor("_EmissionColor", m_Data.material.color);
    }

    public void OnHit()
    {
        // fireworks ?? idk
        if (GameEvents.OnObjectiveHit != null) GameEvents.OnObjectiveHit(ChunkParent);
    }
}
