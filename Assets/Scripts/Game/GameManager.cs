using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int m_Score;
    public int Score { get { return m_Score; } set { m_Score = value; if (GameEvents.OnScoreUpdated != null) GameEvents.OnScoreUpdated(m_Score); } }

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
    }

    public void StartGame()
    {
        if (GameEvents.OnGameStart != null) GameEvents.OnGameStart();

        Chunk firstChunk = ChunkManager.Instance.GetCurrentChunk();
        
        GameEvents.OnTargetColorCombinationUpdated(firstChunk.ColorCombination);
    }

    public void EndGame()
    {
        if (GameEvents.OnGameOver != null) GameEvents.OnGameOver(m_Score);
    }
    
}
