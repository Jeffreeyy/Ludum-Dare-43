using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    public static Fader Instance { get; private set; }

    private Image m_Fader;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        m_Fader = GetComponent<Image>();

        m_Fader.color = new Color(m_Fader.color.r, m_Fader.color.g, m_Fader.color.b, 1);
    }

    public void FadeIn(float time) { Fade(true, time); }
    public void FadeOut(float time) { Fade(false, time); }

    private void Fade(bool fadeIn, float time)
    {
        m_Fader.raycastTarget = true;
        if (GameEvents.OnFaderStart != null) GameEvents.OnFaderStart();
        m_Fader.DOFade(fadeIn ? 1 : 0, time).SetEase(Ease.Linear).OnComplete(() => {
            m_Fader.raycastTarget = fadeIn;
            if (GameEvents.OnFaderEnd != null) GameEvents.OnFaderEnd();
            if (GameEvents.OnFaderStateChanged != null) GameEvents.OnFaderStateChanged(fadeIn);
        });
    }
}