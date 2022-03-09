using BioSim;
using ImageMagick;
using Xunit;
using System;

namespace biosimtests;

public class MapTest
{

    [Fact]
    public void TestConstuct()
    {
        byte[] data = {255, 255, 255};
        CreateTestImage(data);
        Map map = new Map("../tmp/image.png");
    }

    [Fact]
    public void TestDataPixel()
    {
        byte[] data = {255, 255, 255};
        CreateTestImage(data);
        Map map = new Map("../tmp/image.png");

        Assert.Equal<Map.CellType>(Map.CellType.space, map.GetSpot(0, 0));
    }


    [Fact]
    public void TestData()
    {
        Random rnd = new Random();

        byte[] data = new byte[5*5*3];
        rnd.NextBytes(data);
        

        CreateTestImage(data);
        Map map = new Map("../tmp/image.png");
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                
            }
        }
    }

    public void CreateTestImage(byte[] data)
    {
        string tmpDir = "../tmp";
        if (System.IO.Directory.Exists(tmpDir))
            System.IO.Directory.Delete(tmpDir, true);
        System.IO.Directory.CreateDirectory(tmpDir);

        MagickReadSettings mrs = new MagickReadSettings();
        mrs.Format = MagickFormat.Rgb;
        mrs.Width = 50;
        mrs.Height = 50;
        MagickImage mImage = new MagickImage(data, mrs);
        mImage.Write(tmpDir + "/image.png", MagickFormat.Png);
    }
}
