using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private ColorPickup m_ColorPickupPrefab;
    [SerializeField] private ColorObjective m_ColorObjectivePrefab;
    [SerializeField] private Vector2 m_ChunkSize;
    [SerializeField] private int m_MaxPickupsInChunk;
    [SerializeField] private int m_RightCombinationPickupCount;
    [SerializeField] private float m_MinimumSpacingBetweenPickups;

    private List<ColorPickup> m_ActivePickups = new List<ColorPickup>();

    private void Start()
    {
        PreSpawnMap();
    }

    private void PreSpawnMap()
    {
        ColorLibrary library = ColorLibrary.Instance;
        ColorCombination combination = ColorCombinations.GetRandomCombination();
        ColorObjective objective = Instantiate(m_ColorObjectivePrefab, gameObject.transform, false) as ColorObjective;
        objective.transform.position = new Vector3(0, 0, m_ChunkSize.y);

        objective.SetData(library.GetColorItem(combination.output), CollidableType.Objective);

        List<Colors> colorsToSpawn = library.GetColors(false);

        for (int i = 0; i < m_RightCombinationPickupCount; i++)
            CreatePickup(library.GetColorItem(i % 2 == 0 ? combination.color1 : combination.color2), CollidableType.Pickup);

        colorsToSpawn.Remove(combination.color1);
        colorsToSpawn.Remove(combination.color2);

        int maxSpawnAmount = m_MaxPickupsInChunk;
        maxSpawnAmount -= m_RightCombinationPickupCount;

        for (int i = 0; i < maxSpawnAmount; i++)
            CreatePickup(library.GetColorItem(colorsToSpawn[Random.Range(0, colorsToSpawn.Count)]), CollidableType.Pickup);

        GameEvents.OnTargetColorCombinationUpdated(combination);
    }

    private void CreatePickup(ColorItem data, CollidableType type)
    {
        ColorPickup pickup = Instantiate(m_ColorPickupPrefab, gameObject.transform, false) as ColorPickup;
        pickup.transform.position = GetRandomSpawnPosition();
        m_ActivePickups.Add(pickup);
        pickup.SetData(data, type);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos = new Vector3(Random.Range(-m_ChunkSize.x, m_ChunkSize.x), 0, Random.Range(-m_ChunkSize.y, m_ChunkSize.y));

        for (int i = 0; i < m_ActivePickups.Count; i++)
        {
            if (Vector3.Distance(pos, m_ActivePickups[i].transform.position) < m_MinimumSpacingBetweenPickups)
                return GetRandomSpawnPosition();
        }

        return pos;
    }
}
