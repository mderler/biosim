using ImageMagick;

// class to help test with directories
static class TestDirHelper
{
    // create a testimage with provided data and path
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
