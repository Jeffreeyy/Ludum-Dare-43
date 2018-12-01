public enum CollidableType
{
    Pickup,
    Objective
}

public interface ICollidable
{
    CollidableType Type { get; set; }
    Colors GetColor();
    void OnHit();
}