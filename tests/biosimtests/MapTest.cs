using BioSim;
using Xunit;
using System;
namespace biosimtests;

// test the map class
public class MapTest
{
    // path to test image
    private const string _imageDir = "../../../../../tmp/image.png";

    // test construction
    [Fact]
    public void TestConstuct()
    {
        byte[] data = { 255, 255, 255 };
        TestDirHelper.CreateTestImage(data, 1, _imageDir);
        Map map = new Map(_imageDir);
    }

    // test map with one pixel
    [Fact]
    public void TestDataPixel()
    {
        byte[] data = { 255, 255, 255 };
        TestDirHelper.CreateTestImage(data, 1, _imageDir);
        Map map = new Map(_imageDir);

        Assert.Equal<Map.CellType>(Map.CellType.space, map.GetSpot(0, 0));
    }

    // test the convertion between pixel to cells
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

        Map.CellType[] actual = new Map.CellType[5 * 5];

        TestDirHelper.CreateTestImage(data, 5, _imageDir);
        Map map = new Map(_imageDir);

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                int i = x + y * map.Width;
                actual[i] = map.GetSpot(x, y);
            }
        }

        Assert.Equal<Map.CellType[]>(expected, actual);
    }

    // test the exeption
    [Fact]
    public void TestException()
    {
        void CreateMap()
        {
            new Map("");
        }

        Assert.ThrowsAny<Exception>(CreateMap);
    }

    // test reading data
    [Fact]
    public void TestReadData()
    {
        byte[] data =
        {
            255,255,255,
              0,255,  0,
             20, 20, 20
        };

        TestDirHelper.CreateTestImage(data, 1, _imageDir);
        Map map = new Map(_imageDir);
        var actual = map.RawData;

        Assert.Equal(data, actual);
    }
}
