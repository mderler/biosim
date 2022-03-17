using System;
using System.IO;
using ImageMagick;

static class TestDirHelper
{
    public static void CreateTestImage(byte[] data, int width, string dir)
    {
        string? currentDir = Path.GetDirectoryName(dir);
        if (currentDir == null)
        {
            throw new Exception("Directory must not be null");
        }
        if (!Directory.Exists(currentDir))
        {
            Directory.CreateDirectory(currentDir);
        }
        MagickReadSettings mrs = new MagickReadSettings();
        mrs.Format = MagickFormat.Rgb;
        mrs.Width = width;
        mrs.Height = data.Length/3/width;
        MagickImage mImage = new MagickImage(data, mrs);
        mImage.Write(dir, MagickFormat.Png);
    }
}
