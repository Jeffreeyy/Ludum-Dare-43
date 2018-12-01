using UnityEngine;
using DG.Tweening;

public class ColorPickup : MonoBehaviour, ICollidable
{

    private ColorItem m_Data;
    [SerializeField] private Transform m_Handle;
    [SerializeField] private Renderer m_ToColor;

    public CollidableType Type { get; set; }
    public Colors Color { get { return m_Data.color; } }

    private void Start()
    {
        if (m_Handle != null)
            m_Handle.DOLocalRotate(new Vector3(0f, 90f, -m_Handle.localRotation.z), Random.Range(0.33f, 0.66f), RotateMode.Fast).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetData(ColorItem data, CollidableType type)
    {
        m_Data = data;
        Type = type;

        UpdateColor();
    }

    private void UpdateColor()
    {
        m_ToColor.material = m_Data.material;
    }

    public void OnHit()
    {
        m_ToColor.transform.DOScale(0, 0.33f);
    }
}