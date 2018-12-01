using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_TreePrefabs = new List<GameObject>();
    [SerializeField] private int m_TreeAmount = 5;
    [SerializeField] private Vector2 m_TileBounds = new Vector2(5, 5);
    [SerializeField] private float m_OffsetFromTileBounds = 0.5f;
    [Range(0.5f, 3f)]
    [SerializeField] private float m_MinimumSpacingBetweenTrees = 1.5f;
    [SerializeField] private Vector3 m_MinScale = new Vector3(1f, 1.3f, 1f), m_MaxScale = new Vector3(2f, 2.3f, 2f);
    [Space]
    [SerializeField] private bool m_IncludeMapBounds = true;

    private List<GameObject> m_SpawnedTrees = new List<GameObject>();
    private void Start()
    {
        GenerateTrees();
        if(m_IncludeMapBounds)
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

    private void GenerateTrees()
    {
        for (int i = 0; i < m_TreeAmount; i++)
        {
            int randomIndex = Random.Range(0, m_TreePrefabs.Count);
            GameObject tree = Instantiate(m_TreePrefabs[randomIndex], transform, false) as GameObject;
            tree.transform.localPosition = GetRandomSpawnPosition();
            tree.transform.localScale = new Vector3(Random.Range(m_MinScale.x, m_MaxScale.x), Random.Range(m_MinScale.y, m_MaxScale.y), Random.Range(m_MinScale.z, m_MaxScale.z));
            m_SpawnedTrees.Add(tree);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos = new Vector3(Random.Range(-(m_TileBounds.x - m_OffsetFromTileBounds), m_TileBounds.x - m_OffsetFromTileBounds), 0, Random.Range(-(m_TileBounds.y - m_OffsetFromTileBounds), m_TileBounds.y - m_OffsetFromTileBounds));

        for (int i = 0; i < m_SpawnedTrees.Count; i++)
        {
            if (Vector3.Distance(pos, m_SpawnedTrees[i].transform.position) < m_MinimumSpacingBetweenTrees)
                return GetRandomSpawnPosition();
        }

        return pos;
    }
}
