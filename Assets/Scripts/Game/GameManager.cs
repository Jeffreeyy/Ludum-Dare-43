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
        // Pre load all the chunks, gets done once
        ChunkManager.Instance.PreloadChunks();

        StartCoroutine(Intro());
    }

    private IEnumerator Intro()
    {
        Logo.Instance.Show(0.5f);
        yield return new WaitForSeconds(1f);
        Fader.Instance.FadeOut(1f);
    }

    private IEnumerator Restart()
    {
        Logo.Instance.Show(0.5f);
        Fader.Instance.FadeIn(0.5f);
        yield return new WaitForSeconds(0.5f);
        if (GameEvents.OnResetGame != null) GameEvents.OnResetGame();
        yield return new WaitForSeconds(0.5f);
        Fader.Instance.FadeOut(0.5f);
    }

    public void StartGame()
    {
        if (GameEvents.OnGameStart != null) GameEvents.OnGameStart();
        Logo.Instance.Hide(0.5f);
    }

    public void RestartGame()
    {
        Score = 0;
        StartCoroutine(Restart());
    }
}