using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePropSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_PropPrefabs = new List<GameObject>();
    [SerializeField] private int m_MinSpawnAmount = 4, m_MaxSpawnAmount = 5;
    [SerializeField] private Vector2 m_TileBounds = new Vector2(5, 5);
    [SerializeField] private float m_OffsetFromTileBounds = 0.5f;
    [Range(0.5f, 3f)]
    [SerializeField]
    private float m_MinimumSpacingBetweenProps = 1.5f;
    [SerializeField] private bool m_RandomRotation = false;
    [SerializeField] private Vector3 m_MinScale = new Vector3(1f, 1.3f, 1f), m_MaxScale = new Vector3(2f, 2.3f, 2f);

    private List<GameObject> m_SpawnedProps = new List<GameObject>();

    public void Spawn()
    {
        int amount = Random.Range(m_MinSpawnAmount, m_MaxSpawnAmount + 1);
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, m_PropPrefabs.Count);
            GameObject prop = Instantiate(m_PropPrefabs[randomIndex], transform, false) as GameObject;
            prop.transform.localPosition = GetRandomSpawnPosition();
            prop.transform.localScale = new Vector3(Random.Range(m_MinScale.x, m_MaxScale.x), Random.Range(m_MinScale.y, m_MaxScale.y), Random.Range(m_MinScale.z, m_MaxScale.z));
            if (m_RandomRotation)
                prop.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            m_SpawnedProps.Add(prop);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos = new Vector3(Random.Range(-(m_TileBounds.x - m_OffsetFromTileBounds), m_TileBounds.x - m_OffsetFromTileBounds), 0, Random.Range(-(m_TileBounds.y - m_OffsetFromTileBounds), m_TileBounds.y - m_OffsetFromTileBounds));

        for (int i = 0; i < m_SpawnedProps.Count; i++)
        {
            if (Vector3.Distance(pos, m_SpawnedProps[i].transform.position) < m_MinimumSpacingBetweenProps)
                return GetRandomSpawnPosition();
        }

        return pos;
    }
}
