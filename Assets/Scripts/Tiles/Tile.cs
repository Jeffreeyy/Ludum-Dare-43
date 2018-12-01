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

	// Use this for initialization
	void Start ()
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

    private void CreateBound()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider == null)
            collider = gameObject.AddComponent<BoxCollider>();

        collider.isTrigger = true;
        collider.center = new Vector3(0, 5, 0);
        collider.size = new Vector3(10, 10, 10f);

        gameObject.AddComponent<MapBound>();
    }
}
