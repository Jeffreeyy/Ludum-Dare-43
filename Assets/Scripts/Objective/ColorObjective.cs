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
        m_Renderer.material.color = m_Data.material.color;
    }

    public void OnHit()
    {
        // fireworks ?? idk
        if (GameEvents.OnObjectiveHit != null) GameEvents.OnObjectiveHit(ChunkParent);
    }
}
