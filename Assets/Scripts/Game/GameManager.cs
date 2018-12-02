using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ChunkManager.Instance.PreloadChunks();

        StartGame();
    }

    public void StartGame()
    {
        if (GameEvents.OnGameStart != null) GameEvents.OnGameStart();

        Chunk firstChunk = ChunkManager.Instance.GetCurrentChunk();
        
        GameEvents.OnTargetColorCombinationUpdated(firstChunk.ColorCombination);
    }
}
