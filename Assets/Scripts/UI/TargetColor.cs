using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TargetColor : MonoBehaviour
{
    public enum ColorToShow { Color1, Color2, Output }

    private Image m_Image;
    [SerializeField] private ColorToShow m_ColorToShow; 

    private void Awake()
    {
        m_Image = gameObject.GetComponent<Image>();

        GameEvents.OnTargetColorCombinationUpdated += OnTargetColorCombinationUpdated;
    }

    private void OnDestroy()
    {
        GameEvents.OnTargetColorCombinationUpdated -= OnTargetColorCombinationUpdated;
    }

    private void OnTargetColorCombinationUpdated(ColorCombination combination)
    {
        Colors targetColor;
        switch(m_ColorToShow)
        {
            case ColorToShow.Color1:
                targetColor = combination.color1;
                break;
            case ColorToShow.Color2:
                targetColor = combination.color2;
                break;
            default:
            case ColorToShow.Output:
                targetColor = combination.output;
                break;
        }
        m_Image.color = ColorLibrary.Instance.GetColorItem(targetColor).material.color;
    }
}
