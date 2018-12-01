using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreCounter : MonoBehaviour
{
    private Text m_Text;
    private int m_Score;

    private void Awake()
    {
        m_Text = gameObject.GetComponent<Text>();
        OnScoreUpdated(0);

        GameEvents.OnScoreUpdated += OnScoreUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.OnScoreUpdated -= OnScoreUpdated;
    }

    private void OnScoreUpdated(int score)
    {
        m_Score = score;
        m_Text.text = m_Score.ToString();
    }
}
