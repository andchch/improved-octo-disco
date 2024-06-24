using Enums;

public class Tile
{
    public int Bitmask;
    public Tile Bottom;
    public float HeightValue;

    public Tile Left;
    public Tile Right;
    public Tile Top;
    public TileType Type;
    public int X, Y;

    public void SetTileType(float heightValue)
    {
        Type = heightValue switch
        {
            < 0.2f => TileType.DeepWater,
            < 0.4f => TileType.ShallowWater,
            < 0.5f => TileType.Sand,
            < 0.7f => TileType.Grass,
            < 0.8f => TileType.Forest,
            < 0.9f => TileType.Mountain,
            _ => TileType.Snow
        };
    }

    public void UpdateBitmask()
    {
        var count = 0;

        if (Top.Type == Type)
            count += 1;
        if (Right.Type == Type)
            count += 2;
        if (Bottom.Type == Type)
            count += 4;
        if (Left.Type == Type)
            count += 8;

        Bitmask = count;
    }
}