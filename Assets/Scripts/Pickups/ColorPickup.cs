using UnityEngine;
using DG.Tweening;

public class ColorPickup : MonoBehaviour, ICollidable
{

    private ColorItem m_Data;
    [SerializeField] private Transform m_Handle;
    [SerializeField] private Renderer m_ToColor;
    [Space]
    [Header("Idle Animation")]
    [SerializeField] private bool m_AnimateHandle = true;
    [SerializeField] private bool m_AnimateScale = true;
    [SerializeField] private bool m_AnimatePosition = true;

    public CollidableType Type { get; set; }
    public Colors Color { get { return m_Data.color; } }
    public bool BeenHit { get; set; }

    private void AnimateIdle()
    {
        KillTweens();

        float animateSpeed = Random.Range(1.2f, 1.5f);

        if (m_AnimateHandle)
        {
            if (m_Handle != null)
                m_Handle.DOLocalRotate(new Vector3(0f, 90f, 10), animateSpeed / 2, RotateMode.Fast).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        if(m_AnimateScale)
            transform.DOScale(new Vector3(Random.Range(0.9f, 1f), Random.Range(0.9f, 1f), Random.Range(0.9f, 1f)), animateSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        if(m_AnimatePosition)
            transform.DOLocalMoveY(0.2f, animateSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void KillTweens()
    {
        m_Handle.DOKill();
        transform.DOKill();
        m_ToColor.DOKill();
    }

    private void SetDefaultObjectValues()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        m_ToColor.transform.localScale = Vector3.one;
    }

    public void SetData(ColorItem data, CollidableType type)
    {
        m_Data = data;
        Type = type;
        BeenHit = false;

        UpdateColor();
        SetDefaultObjectValues();
        AnimateIdle();
    }

    private void UpdateColor()
    {
        m_ToColor.material = m_Data.material;
    }

    public void OnHit()
    {
        BeenHit = true;
        m_ToColor.transform.DOScale(0, 0.33f);
    }
}