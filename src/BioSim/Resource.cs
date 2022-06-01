using ImageMagick;

namespace BioSim;

public static class Resource
{
    public const string RelativeBasePath = "../../res/";

    public static MagickImage LoadImageFromResource(string path)
    {

        return new MagickImage(RelativeBasePath + path);
    }

    public static string CombinePath(string path)
    {
        return RelativeBasePath + path;
    }
}
