using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorCombination
{
    public Colors color1;
    public Colors color2;
    public Colors output;

    public ColorCombination(Colors color1, Colors color2, Colors output)
    {
        this.color1 = color1;
        this.color2 = color2;
        this.output = output;
    }
}

public class ColorCombinations
{
    public static List<ColorCombination> Combinations = new List<ColorCombination>()
    {
        new ColorCombination(Colors.Yellow, Colors.Red, Colors.Orange)
    };

    public static Colors GetCombinedColor(Colors color1, Colors color2)
    {
        try
        {
            for (int i = 0; i < Combinations.Count; i++)
            {
                if (Combinations[i].color1 == color1 && Combinations[i].color2 == color2 || 
                    Combinations[i].color1 == color2 && Combinations[i].color2 == color1)
                    return Combinations[i].output;
            }

            throw new KeyNotFoundException("Combination does not exist");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return Colors.White;
        }
    }

    public static ColorCombination GetRandomCombination()
    {
        try
        {
            if(Combinations.Count > 0)
                return Combinations[UnityEngine.Random.Range(0, Combinations.Count)];
            else
                throw new Exception("Tried to get a random Color Combination but the list is empty. Please add combinations to the ColorCombinations.cs script");
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }
}