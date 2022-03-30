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

    // TODO: Make a config file contaning interpetrations
    private readonly (byte r, byte g, byte b, byte c)[] _defaultMappings =
    {
        (255, 255, 255, 0),
        (  0, 255,   0, 1),
        ( 20,  20,  20, 2)
    };

    private CellType[][] _mapData;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Map(string path)
    {
        MagickImage image = new MagickImage(path);
        Width = image.Width;
        Height = image.Height;

        _mapData = new CellType[Height][];

        Interpetrate(image);
    }

    // TODO: finish this method
    public void ReadMapping(string path)
    {
        StreamReader sr = new StreamReader(path);
        string text = sr.ReadToEnd();

        string[] data = text.Split(',');
    }

    private void Interpetrate(MagickImage image)
    {
        byte[]? data = image.GetPixels().ToByteArray(0, 0, Width, Height, "RGB");

        if (data == null)
            throw new Exception("Image data must not be null");
        

        for (int i = 0; i < data.Length; i+=3)
        {
            int p = i/3;
            int y = p/Width;
            int x = p%Width;

            if (x==0)
                _mapData[y] = new CellType[Width];

            // Is the score high, the mapping is bad 
            int bestScore = int.MaxValue;
            foreach (var item in _defaultMappings)
            {
                int score = 0;
                score += Math.Abs(item.r-data[i]);
                score += Math.Abs(item.g-data[i+1]);
                score += Math.Abs(item.b-data[i+2]);

                if (score <= bestScore)
                {
                    _mapData[y][x] = (CellType)item.c;
                    bestScore = score;
                }
            }
        }
    }

    public CellType GetSpot(int x, int y)
    {
        return _mapData[y][x];
    }

    public CellType GetSpot((int x, int y) position)
    {
        return _mapData[position.y][position.x];
    }

    public void SetSpot((int x, int y) position, CellType cellType)
    {
        _mapData[position.y][position.x] = cellType;
    }

    public int FreeSpaceCount
    {
        get
        {
            int count = 0;
            foreach (var row in _mapData)
            {
                foreach (var item in row)
                {
                    if (item == CellType.space)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }

    public byte[] ReadData()
    {
        int len = Width*Height;
        byte[] data = new byte[len*3];

        for (int i = 0; i < len; i++)
        {
            CellType cell = _mapData[i/Width][i%Width];
            foreach (var item in _defaultMappings)
            {
                if (item.c == (byte)cell)
                {
                    int j = i*3;
                    data[j] = item.r;
                    data[j+1] = item.g;
                    data[j+2] = item.b;
                    break;
                }
            }
        }

        return data;
    }
}
