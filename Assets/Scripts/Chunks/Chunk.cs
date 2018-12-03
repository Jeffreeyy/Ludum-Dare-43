using UnityEngine;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
    [SerializeField] private ColorObjective m_Objective;
    [SerializeField] private ColorPickup m_ColorPickupPrefab;
    [Header("Tiles to spawn on")]
    [SerializeField] private List<Tile> m_Tiles;
    [Header("Spawn Amounts")]
    [SerializeField] private int m_MinSpawnAmount = 18;
    [SerializeField] private int m_MaxSpawnAmount = 22;
    [Header("Right combination spawn percentage")]
    [Range(0, 100)]
    [SerializeField] private float m_Percentage = 70f;
    [SerializeField] private float m_MinimumSpacingBetweenPickups = 2f;
    [Space]
    [SerializeField] private List<ChunkAdditive> m_Additives;

    public bool InUse { get; set; }

    public ColorCombination ColorCombination { get; set; }

    private const string PICKUP_IDENTIFIER = "Pickups";

    private Pool m_PickupPool;
    private List<ColorPickup> m_SpawnedPickups = new List<ColorPickup>();

    private void Awake()
    {
        m_PickupPool = ObjectPool.Instance.CreatePool(m_ColorPickupPrefab.gameObject, 25, PICKUP_IDENTIFIER);
    }

    public void CreatePickups()
    {
        // Make sure that the reference to the ColorLibrary Instance is there
        if (ColorLibrary.Instance == null) return;

        // Get a random color combination
        ColorCombination = ColorCombinations.GetRandomCombination();

        // Create holder
        GameObject pickupHolder = new GameObject(PICKUP_IDENTIFIER);
        pickupHolder.transform.SetParent(transform);

        // Set the objective data for this chunk
        m_Objective.SetData(ColorLibrary.Instance.GetColorItem(ColorCombination.output), CollidableType.Objective);
        m_Objective.ChunkParent = this;

        // Get a list of colors that can be spawned
        List<Colors> colorsToSpawn = ColorLibrary.Instance.GetColors(false);

        // Get a random pickup spawn amount
        int spawnAmount = Random.Range(m_MinSpawnAmount, m_MaxSpawnAmount);
        // Make 80% of the pickup spawn amount the right combination
        int rightCombinationSpawnAmount = Mathf.RoundToInt(spawnAmount * (m_Percentage / 100));
        // Update the spawn amount
        spawnAmount -= rightCombinationSpawnAmount;
        // Shuffle the tiles
        ShuffleTiles();

        // Spawn the right combination pickups
        SpawnCorrectCombinationPickups(ColorCombination, rightCombinationSpawnAmount, pickupHolder.transform);

        // Remove the combination colors
        colorsToSpawn.Remove(ColorCombination.color1);
        colorsToSpawn.Remove(ColorCombination.color2);

        // Spawn the rest of the pickups
        SpawnOtherPickups(ColorCombination, spawnAmount, colorsToSpawn, pickupHolder.transform);
    }

    public void ClearPickups()
    {
        for (int i = m_SpawnedPickups.Count - 1; i >= 0; i--)
            m_PickupPool.Despawn(m_SpawnedPickups[i].gameObject);

        m_SpawnedPickups.Clear();
    }

    private ColorPickup CreatePickup(ColorItem data, CollidableType type, Transform parent)
    {
        ColorPickup pickup = m_PickupPool.Get().GetComponent<ColorPickup>();
        pickup.transform.SetParent(parent);
        pickup.SetData(data, type);
        m_SpawnedPickups.Add(pickup);

        return pickup;
    }

    private Vector3 GetRandomSpawnPosition(Tile tile)
    {
        Vector3 pos = new Vector3(Random.Range(-3.5f, 3.5f), 0, Random.Range(-3.5f, 3.5f));
        Vector3 calculatedWorldPos = tile.transform.position + pos;

        for (int i = 0; i < m_SpawnedPickups.Count; i++)
        {
            if (Vector3.Distance(calculatedWorldPos, m_SpawnedPickups[i].transform.position) < m_MinimumSpacingBetweenPickups)
                return GetRandomSpawnPosition(tile);
        }

        return calculatedWorldPos;
    }

    private void ShuffleTiles()
    {
        System.Random rng = new System.Random();
        int n = m_Tiles.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Tile value = m_Tiles[k];
            m_Tiles[k] = m_Tiles[n];
            m_Tiles[n] = value;
        }
    }

    private void SpawnCorrectCombinationPickups(ColorCombination combination, int rightCombinationSpawnAmount, Transform pickupHolder)
    {
        for (int i = 0; i < m_Tiles.Count; i++)
        {
            if (rightCombinationSpawnAmount <= 0) break;

            ColorPickup pickup = CreatePickup(ColorLibrary.Instance.GetColorItem(i % 2 == 0 ? combination.color1 : combination.color2), CollidableType.Pickup, pickupHolder);
            pickup.transform.position = GetRandomSpawnPosition(m_Tiles[i]);

            rightCombinationSpawnAmount--;
        }

        if (rightCombinationSpawnAmount > 0)
            SpawnCorrectCombinationPickups(combination, rightCombinationSpawnAmount, pickupHolder);
    }

    private void SpawnOtherPickups(ColorCombination combination, int spawnAmount, List<Colors> colorsToSpawn, Transform pickupHolder)
    {
        for (int i = 0; i < m_Tiles.Count; i++)
        {
            if (spawnAmount <= 0) break;

            ColorPickup pickup = CreatePickup(ColorLibrary.Instance.GetColorItem(colorsToSpawn[Random.Range(0, colorsToSpawn.Count)]), CollidableType.Pickup, pickupHolder);
            pickup.transform.position = GetRandomSpawnPosition(m_Tiles[i]);

            spawnAmount--;
        }

        if (spawnAmount > 0)
            SpawnOtherPickups(combination, spawnAmount, colorsToSpawn, pickupHolder);
    }

    public void ShowAdditives()
    {
        if (m_Additives.Count <= 0) return;

        for (int i = 0; i < m_Additives.Count; i++)
            m_Additives[i].Show();
    }
}
