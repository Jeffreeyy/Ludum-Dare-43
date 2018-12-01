public enum CollidableType
{
    Pickup,
    Objective,
    Bound
}

public interface ICollidable
{
    CollidableType Type { get; set; }
    Colors Color { get; }
    void SetData(ColorItem data, CollidableType type);
    void OnHit();
}