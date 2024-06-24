using System.Collections.Generic;
using Enums;

public class TileGroup
{
    public List<Tile> Tiles;
    public TileGroupType Type;

    public TileGroup()
    {
        Tiles = new List<Tile>();
    }
}