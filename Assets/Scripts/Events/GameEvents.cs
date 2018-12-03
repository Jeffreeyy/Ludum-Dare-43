using System;

public class GameEvents
{
    public static Action OnGameStart;
    public static Action OnResetGame;
    public static Action<int> OnGameOver;

    public static Action OnMapBoundHit;
    public static Action OnMapBoundLeave;


    public static Action<Chunk> OnObjectiveHit;

    public static Action<int> OnScoreUpdated;
    public static Action<ColorCombination> OnTargetColorCombinationUpdated;

    public static Action OnFaderStart;
    public static Action OnFaderEnd;
    public static Action<bool> OnFaderStateChanged;
}
