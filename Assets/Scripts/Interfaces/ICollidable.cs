public enum CollidableType
{
    Pickup,
    Objective
}

public interface ICollidable
{
    CollidableType Type { get; set; }
    Colors Color { get; }
    bool BeenHit { get; set; }
    void SetData(ColorItem data, CollidableType type);
    void OnHit();
}