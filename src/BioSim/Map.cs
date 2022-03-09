using ImageMagick;

namespace BioSim;

public class Map
{
    public enum CellType
    {
        space = 0,
        survive = 1,
        wall = 2
    }

    // TODO: test if a 1-dim. Array with x+y*width would be faster
    private CellType[][] _mapData;
 
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Map(string path)
    {
        // TODO: Make a config file contaning interpretations
        (byte r, byte g, byte b, byte c)[] interp =
        {
            (255, 255, 255, 0),
            (  0,   0,   0, 0),
            (  0, 255,   0, 1),
            ( 20,  20,  20, 2)
        };

        MagickImage image = new MagickImage(path);
        Width = image.Width;
        Height = image.Height;
        byte[] data = image.ToByteArray();

        _mapData = new CellType[Height][];

        for (int i = 0; i < data.Length; i+=3)
        {
            int p = i/3;
            int y = p/Width;
            int x = p-y*Width;

            _mapData[p] = new CellType[Width];

            (byte r, byte g, byte b) pixel = (data[i], data[i+1], data[1+2]);
           foreach (var item in interp)
           {
               if ((item.r, item.g, item.b) == pixel)
               {
                   _mapData[y][x] = (CellType)item.c;
               }
           }
        }
        
    }

    public CellType GetSpot(int x, int y)
    {
        return _mapData[y][x];
    }
}