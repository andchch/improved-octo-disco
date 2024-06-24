using System.Collections.Generic;
using Enums;
using NoiseLib.Enums;
using NoiseLib.Implicit;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")] [SerializeField]
    public int Width = 128;

    [SerializeField] public int Height = 128;

    [Header("Noise Settings")] [SerializeField]
    private int TerrainOctaves = 6;

    [SerializeField] private double TerrainFrequency = 1.25;
    [SerializeField] private int Seed;

    [Space] [Header("Prefabs")] [SerializeField]
    private GameObject deepWater;

    [SerializeField] private GameObject shallowWater;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grass;
    [SerializeField] private GameObject forest;
    [SerializeField] private GameObject mountain;
    [SerializeField] private GameObject snow;

    private Dictionary<TileType, GameObject> _tileGroups;
    private Dictionary<TileType, GameObject> _tileSet;

    private MapData HeightData;
    private ImplicitFractal HeightMap;
    private List<TileGroup> Lands = new();
    public Tile[,] Tiles;
    private List<TileGroup> Waters = new();

    private void Start()
    {
        Initialize();
        GetData(ref HeightData);
        LoadTiles();
        CreateTileset();
        CreateTileGroups();
        GenerateMap(ref HeightData);
    }

    private void Initialize()
    {
        // Инициализируем генератор карты высот
        HeightMap = new ImplicitFractal(
            FractalType.MULTI,
            BasisType.SIMPLEX,
            InterpolationType.QUINTIC,
            TerrainOctaves,
            TerrainFrequency,
            Seed);
    }

    private void GetData(ref MapData mapData)
    {
        mapData = new MapData(Width, Height);
        // проходим по каждой точке x,y - получаем значение высоты
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            //Сэмплируем шум с небольшими интервалами
            var x1 = x / (float) Width;
            var y1 = y / (float) Height;

            var value = (float) HeightMap.Get(x1, y1);

            //отслеживаем максимальные и минимальные найденные значения
            if (value > mapData.Max) mapData.Max = value;
            if (value < mapData.Min) mapData.Min = value;

            mapData.Data[x, y] = value;
        }
    }

    private void LoadTiles()
    {
        // Создаем массив тайлов
        Tiles = new Tile[Width, Height];

        // На каждой новой клетке создаем тайл
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var t = new Tile
            {
                X = x,
                Y = y
            };

            var value = HeightData.Data[x, y];

            //нормализуем наше значение от 0 до 1
            value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);

            t.HeightValue = value;
            t.SetTileType(value);

            Tiles[x, y] = t;
        }
    }

    private void GenerateMap(ref MapData map)
    {
        // Проходим по каждой клетке и создаем тайл
        for (var x = 0; x < map.Width; x++)
        for (var y = 0; y < map.Height; y++)
            CreateTile(Tiles[x, y]);
    }

    private void CreateTileGroups()
    {
        _tileGroups = new Dictionary<TileType, GameObject>();
        foreach (var prefabPair in _tileSet)
        {
            var tileGroup = new GameObject(prefabPair.Value.name)
            {
                transform =
                {
                    parent = gameObject.transform,
                    localPosition = new Vector3(0, 0, 0)
                }
            };
            _tileGroups.Add(prefabPair.Key, tileGroup);
        }
    }

    private void CreateTileset()
    {
        _tileSet = new Dictionary<TileType, GameObject>
        {
            {TileType.DeepWater, deepWater},
            {TileType.ShallowWater, shallowWater},
            {TileType.Sand, sand},
            {TileType.Grass, grass},
            {TileType.Forest, forest},
            {TileType.Mountain, mountain},
            {TileType.Snow, snow}
        };
    }

    private void CreateTile(Tile t)
    {
        var tilePrefab = _tileSet[t.Type];
        var tileGroup = _tileGroups[t.Type];
        var tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name = $"tile_x{t.X}_y{t.Y}";
        tile.transform.localPosition = new Vector3(t.X, t.Y, 0);
    }

    private void UpdateNeighbors()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            Tiles[x, y].Top = GetTop(Tiles[x, y]);
            Tiles[x, y].Bottom = GetBottom(Tiles[x, y]);
            Tiles[x, y].Left = GetLeft(Tiles[x, y]);
            Tiles[x, y].Right = GetRight(Tiles[x, y]);
        }
    }

    private void UpdateBitmasks()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            Tiles[x, y].UpdateBitmask();
    }

    #region GetTilesByDirection

    private Tile GetTop(Tile t)
    {
        return Tiles[t.X, t.Y + 1];
    }

    private Tile GetBottom(Tile t)
    {
        return Tiles[t.X, t.Y - 1];
    }

    private Tile GetLeft(Tile t)
    {
        return Tiles[t.X - 1, t.Y];
    }

    private Tile GetRight(Tile t)
    {
        return Tiles[t.X + 1, t.Y];
    }

    #endregion
}