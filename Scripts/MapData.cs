public class MapData
{
    public float[,] Data;
    public int Height;
    public int Width;

    public MapData(int width, int height)
    {
        Data = new float[width, height];
        Width = width;
        Height = height;
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public float Min { get; set; }
    public float Max { get; set; }
}