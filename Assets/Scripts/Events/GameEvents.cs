using System;

public class GameEvents
{
    public static Action OnMapBoundHit;
    public static Action OnMapBoundLeave;

    public static Action<int> OnScoreUpdated;
    public static Action<ColorCombination> OnTargetColorCombinationUpdated;
}
