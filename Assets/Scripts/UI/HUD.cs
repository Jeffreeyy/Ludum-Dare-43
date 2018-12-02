using UnityEngine;

public class HUD : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.OnGameStart += Show;
        GameEvents.OnGameOver += Hide;

        Hide();
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= Show;
        GameEvents.OnGameOver -= Hide;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide(int arg1 = 0)
    {
        gameObject.SetActive(false);
    }
}
