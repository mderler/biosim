public class Map
{
    private byte[][] _mapData;
 
    private int _width;
    private int _height;

    public int Width => _width;
    public int Height => _height;
    public Map(string path)
    {
        _mapData = new byte[0][];
    }

    public byte GetSpot(int x, int y)
    {
        return _mapData[y][x];
    }
}