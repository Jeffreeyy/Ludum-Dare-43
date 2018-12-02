using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool m_Trees;
    [SerializeField] private bool m_Grass;
    [SerializeField] private bool m_Sticks;
    [SerializeField] private bool m_Bounding;
    [Space]
    [SerializeField] private TilePropSpawner m_TreeSpawner;
    [SerializeField] private TilePropSpawner m_GrassSpawner;
    [SerializeField] private TilePropSpawner m_SticksSpawner;

    private BoxCollider m_Collider;
    private MapBound m_MapBound;

    public void Generate()
    {
        if (m_Trees)
            m_TreeSpawner.Spawn();

        if (m_Grass)
            m_GrassSpawner.Spawn();

        if (m_Sticks)
            m_SticksSpawner.Spawn();

        if (m_Bounding)
            CreateBound();
    }

    public void Clear()
    {
        m_TreeSpawner.Clear();
        m_GrassSpawner.Clear();
        m_SticksSpawner.Clear();

        RemoveBound();
    }

    private void CreateBound()
    {
        m_Collider = GetComponent<BoxCollider>();
        if (m_Collider == null)
            m_Collider = gameObject.AddComponent<BoxCollider>();

        m_Collider.isTrigger = true;
        m_Collider.center = new Vector3(0, 5, 0);
        m_Collider.size = new Vector3(10, 10, 10f);

        if(m_MapBound == null)
            m_MapBound = gameObject.AddComponent<MapBound>();
    }

    private void RemoveBound()
    {
        if (m_MapBound != null)
            DestroyImmediate(m_MapBound);

        m_MapBound = null;

        if (m_Collider != null)
            DestroyImmediate(m_Collider);

        m_Collider = null;
    }
}
