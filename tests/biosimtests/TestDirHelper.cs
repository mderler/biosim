using System;
using System.IO;
using ImageMagick;

static class TestDirHelper
{
    public static void CreateTestImage(byte[] data, int width, string filename)
    {
        MagickReadSettings mrs = new MagickReadSettings();
        mrs.Format = MagickFormat.Rgb;
        mrs.Width = width;
        mrs.Height = data.Length / 3 / width;
        MagickImage mImage = new MagickImage(data, mrs);
        mImage.Write(filename, MagickFormat.Png);
    }
}
