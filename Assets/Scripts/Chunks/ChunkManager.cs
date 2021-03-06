﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager Instance { get; private set; }

    [SerializeField] private List<Chunk> m_ChunkPrefabs = new List<Chunk>();
    [SerializeField] private int m_AmountOfPreloadedChunksPerChunk = 2;
    [SerializeField] private int m_AmountOfChunksVisible = 2;
    private float m_ChunkSize = 60f;
    private int m_SpawnedChunkIndex = 0;

    private Chunk m_CurrentChunk;
    private Transform m_VisibleChunkHolder;
    private List<Chunk> m_VisibleChunks = new List<Chunk>();

    private List<Chunk> m_ChunkPool = new List<Chunk>();
    private Transform m_ChunkPoolHolder;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameEvents.OnGameStart += OnGameStart;
        GameEvents.OnObjectiveHit += OnObjectiveHit;
        GameEvents.OnResetGame += OnReset;

        m_VisibleChunkHolder = new GameObject("Visible Chunks").transform;
        m_VisibleChunkHolder.SetParent(transform);
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= OnGameStart;
        GameEvents.OnObjectiveHit -= OnObjectiveHit;
        GameEvents.OnResetGame -= OnReset;
    }

    private void OnGameStart()
    {
        StartCoroutine(ShowFirstChunkAdditivesDelayed());
    }

    private IEnumerator ShowFirstChunkAdditivesDelayed()
    {
        yield return new WaitForSeconds(2f);
        m_CurrentChunk.ShowAdditives();
    }

    private void OnReset()
    {
        for (int i = 0; i < m_VisibleChunks.Count; i++)
            DespawnChunk(m_VisibleChunks[i]);

        m_VisibleChunks.Clear();

        SpawnFirstChunks();
    }

    private void OnObjectiveHit(Chunk completedChunk)
    {
        if(completedChunk == m_VisibleChunks[1])
        {
            DespawnChunk(m_VisibleChunks[0]);
            m_VisibleChunks.RemoveAt(0);

            SpawnRandomChunk();
        }
        m_CurrentChunk = m_VisibleChunks[1];
        m_CurrentChunk.ShowAdditives();
        if(GameEvents.OnTargetColorCombinationUpdated != null)
            GameEvents.OnTargetColorCombinationUpdated(m_CurrentChunk.ColorCombination);
    }

    private void SpawnFirstChunks()
    {
        m_SpawnedChunkIndex = 0;

        for (int i = 0; i < m_AmountOfChunksVisible; i++)
            SpawnRandomChunk();

        m_CurrentChunk = m_VisibleChunks[0];

        if (GameEvents.OnTargetColorCombinationUpdated != null)
            GameEvents.OnTargetColorCombinationUpdated(m_CurrentChunk.ColorCombination);
    }

    public void SpawnRandomChunk()
    {
        Chunk randomChunk = GetRandomChunk();
        randomChunk.InUse = true;
        randomChunk.CreatePickups();
        randomChunk.transform.SetParent(m_VisibleChunkHolder);
        randomChunk.transform.localPosition = new Vector3(0, 0, m_SpawnedChunkIndex * m_ChunkSize);

        m_VisibleChunks.Add(randomChunk);
        m_SpawnedChunkIndex++;
    }

    public void DespawnChunk(Chunk chunk)
    {
        chunk.ClearPickups();
        chunk.InUse = false;
        chunk.transform.SetParent(m_ChunkPoolHolder);
        chunk.transform.localPosition = Vector3.zero;
    }

    public Chunk GetCurrentChunk()
    {
        return m_CurrentChunk;
    }


    private Chunk GetRandomChunk()
    {
        try
        {
            if (m_ChunkPrefabs.Count <= 0)
                throw new UnassignedReferenceException("List of chunk prefabs is empty, please assign them in the inspector");

            int randomChunkIndex = Random.Range(0, m_ChunkPrefabs.Count);

            return GetChunkFromPool(m_ChunkPrefabs[randomChunkIndex]);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    private Chunk GetChunkFromPool(Chunk chunkPrefab)
    {
        for (int i = 0; i < m_ChunkPool.Count; i++)
        {
            if (m_ChunkPool[i].name == chunkPrefab.name && !m_ChunkPool[i].InUse)
                return m_ChunkPool[i];
        }
        return CreateChunk(chunkPrefab);
    }

    public void PreloadChunks()
    {
        m_ChunkPoolHolder = new GameObject("Preloaded Chunks").transform;
        m_ChunkPoolHolder.SetParent(transform);
        m_ChunkPoolHolder.localPosition = new Vector3(0, 0, -250);

        for (int i = 0; i < m_ChunkPrefabs.Count; i++)
        {
            for (int j = 0; j < m_AmountOfPreloadedChunksPerChunk; j++)
                CreateChunk(m_ChunkPrefabs[i]);
        }

        SpawnFirstChunks();
    }

    private Chunk CreateChunk(Chunk prefab)
    {
        Chunk chunk = Instantiate(prefab, m_ChunkPoolHolder, false) as Chunk;
        chunk.name = chunk.name.Replace("(Clone)", "");
        chunk.InUse = false;
        chunk.transform.localPosition = Vector3.zero;
        m_ChunkPool.Add(chunk);
        return chunk;
    }
}
