using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickup : MonoBehaviour {

    [SerializeField] private ColorItem m_Data;
    private Renderer m_Renderer;

    private void Start()
    {
        m_Renderer = gameObject.GetComponent<Renderer>();
        UpdateColor();
    }

    private void UpdateColor()
    {
        m_Renderer.material = m_Data.material;
    }

    public void OnPickup()
    {
        m_Renderer.material = ColorLibrary.Instance.GetMaterial(Colors.White);
    }

    public Colors GetColor()
    {
        return m_Data.color;
    }
}
