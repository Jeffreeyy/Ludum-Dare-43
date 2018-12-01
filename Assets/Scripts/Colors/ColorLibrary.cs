using System;
using UnityEngine;
using System.Collections.Generic;

public enum Colors
{
    Green,
    Red,
    Blue,
    Yellow,
    Purple,
    Orange,
    White
}

[Serializable]
public class ColorItem
{
    public Colors color;
    public Material material;
}

public class ColorLibrary : MonoBehaviour
{
    public static ColorLibrary Instance { get; private set; }
    [SerializeField] private List<ColorItem> m_Colors;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public ColorItem GetRandomColor()
    {
        try
        {
            if(m_Colors.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, m_Colors.Count);
                return m_Colors[random];
            }
            else
                throw new Exception("Tried to get a random color from the Color Library but it's empty. Please assign values in the inspector");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    public ColorItem GetColorItem(Colors color)
    {
        try
        {
            ColorItem colorItem = m_Colors.Find(x => x.color == color);

            if (colorItem != null)
                return colorItem;
            else
                throw new KeyNotFoundException("Given color cannot be found in the Color Library. Please make sure it's added in the inspector");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    public List<Colors> GetColors()
    {
        try
        {
            List<Colors> temp = new List<Colors>();

            for (int i = 0; i < m_Colors.Count; i++)
                temp.Add(m_Colors[i].color);

            if (temp.Count > 0)
                return temp;
            else
                throw new Exception("Tried to get all the colors from the Color Library but it's empty. Please assign values in the inspector");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    public Material GetMaterial(Colors color)
    {
        try
        {
            ColorItem colorItem = GetColorItem(color);

            if (colorItem != null)
                return colorItem.material;
            else
                throw new KeyNotFoundException("Given color cannot be found in the Color Library. Please make sure it's added in the inspector");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }
}
