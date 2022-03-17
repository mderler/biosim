using BioSim;
using ImageMagick;
using Xunit;
using System;
using System.IO;

namespace biosimtests;

public class MapTest
{
    private const string _imageDir = "../tmp/image.png";

    [Fact]
    public void TestConstuct()
    {
        byte[] data = {255, 255, 255};
        TestDirHelper.CreateTestImage(data, 1, _imageDir);
        Map map = new Map(_imageDir);
    }

    [Fact]
    public void TestDataPixel()
    {
        byte[] data = {255, 255, 255};
        TestDirHelper.CreateTestImage(data, 1, _imageDir);
        Map map = new Map(_imageDir);

        Assert.Equal<Map.CellType>(Map.CellType.space, map.GetSpot(0, 0));
    }

    [Fact]
    public void TestData()
    {
        byte[] data = 
        {
              0,   0,   0,
            255, 255, 255,
              0, 255,   0,
             22,  22,  22,
             33,  33,  33,
             66,  66,  66,
             51, 200,  70,
             37,  70,  80,
              0,   0,   0,
            255, 255, 255,
              0, 255,   0,
             22,  22,  22,
             33,  33,  33,
             66,  66,  66,
             51, 200,  70,
             37,  70,  80,
              0,   0,   0,
            255, 255, 255,
              0, 255,   0,
             22,  22,  22,
             33,  33,  33,
             66,  66,  66,
             51, 200,  70,
             37,  70,  80,
            221, 250, 230,
        };
        
        Map.CellType[] expected =
        {
            Map.CellType.wall,
            Map.CellType.space,
            Map.CellType.survive,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.survive,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.space,
            Map.CellType.survive,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.survive,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.space,
            Map.CellType.survive,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.wall,
            Map.CellType.survive,
            Map.CellType.wall,
            Map.CellType.space
        };

        Map.CellType[] actual = new Map.CellType[5*5];

        TestDirHelper.CreateTestImage(data, 5, _imageDir);
        Map map = new Map(_imageDir);

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                int i = x+y*map.Width;
                actual[i] = map.GetSpot(x, y);
            }
        }

        Assert.Equal<Map.CellType[]>(expected, actual);
    }

    [Fact]
    public void TestException()
    {
        void CreateMap()
        {
            new Map("");
        }

        Assert.ThrowsAny<Exception>(CreateMap);
    }
}
