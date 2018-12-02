using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.OnGameStart += Hide;
        GameEvents.OnGameOver += Show;

        Show();
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= Hide;
        GameEvents.OnGameOver -= Show;
    }

    private void Show(int arg1 = 0)
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        GameManager.Instance.StartGame();
    }
}
