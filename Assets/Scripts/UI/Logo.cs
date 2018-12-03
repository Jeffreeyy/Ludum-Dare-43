using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Logo : MonoBehaviour
{
    public static Logo Instance { get; private set; }

    private Image m_Logo;
    private RectTransform m_RectTransform;

    [SerializeField] private Vector2 m_StartPos, m_MidPos, m_EndPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        m_Logo = GetComponent<Image>();
        m_RectTransform = GetComponent<RectTransform>();

        SetDefaultValues(true);
    }

    public void Show(float time)
    {
        SetDefaultValues(true);
        m_Logo.DOFade(1, time / 3).SetEase(Ease.Linear);
        m_RectTransform.DOAnchorPos(m_MidPos, time).SetEase(Ease.OutExpo);
    }

    public void Hide(float time)
    {
        SetDefaultValues(false);
        m_Logo.DOFade(0, time / 3).SetEase(Ease.Linear).SetDelay(time - (time / 3));
        m_RectTransform.DOAnchorPos(m_EndPos, time).SetEase(Ease.InExpo);
    }

    private void SetDefaultValues(bool defaultForShow)
    {
        m_RectTransform.DOKill();
        m_Logo.DOKill();

        m_RectTransform.anchoredPosition = defaultForShow ? m_StartPos : m_MidPos;
        m_Logo.color = new Color(m_Logo.color.r, m_Logo.color.g, m_Logo.color.b, defaultForShow ? 0 : 1);
    }
}
