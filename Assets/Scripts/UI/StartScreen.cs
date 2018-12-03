using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private bool m_GameOver;

    private void Awake()
    {
        GameEvents.OnGameStart += OnGameStart;
        GameEvents.OnResetGame += OnResetGame;
        GameEvents.OnGameOver += OnGameOver;

        Show();
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= OnGameStart;
        GameEvents.OnResetGame -= OnResetGame;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void OnGameStart()
    {
        m_GameOver = false;
        Hide();
    }

    private void OnResetGame()
    {
        m_GameOver = false;
    }

    private void OnGameOver(int score)
    {
        m_GameOver = true;
        Show();
    }

    private void Show() { gameObject.SetActive(true); }
    private void Hide() { gameObject.SetActive(false); }

    public void OnClick()
    {
        if (m_GameOver)
            GameManager.Instance.RestartGame();
        else
            GameManager.Instance.StartGame();
    }
}