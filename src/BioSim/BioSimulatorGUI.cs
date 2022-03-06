using Gtk;
using ImageMagick;

class BioSimulatorGUI
{
    private BioSimWindow _window;
    private readonly FunctionFactory _simulationFactory;

    public BioSimulatorGUI()
    {
        _simulationFactory = new FunctionFactory();
        _simulationFactory.RegisterIOFunction("Input Test", InputFunctions.GetOneTest);
        _simulationFactory.RegisterIOFunction("Output Test", OutputFunctions.PrintPositionTest);
        _window = new BioSimWindow();
        
        NewSimButton newSimButton = new NewSimButton();
        _window.Add(newSimButton);
        
        _window.ShowAll();
    }

    public void ImageTest()
    {
        string tmpDir = "../../tmp";
        if (System.IO.Directory.Exists(tmpDir))
            System.IO.Directory.Delete(tmpDir, true);
        System.IO.Directory.CreateDirectory(tmpDir);

        byte[] imageData = new byte[50*50*3];
        for (int i = 0; i < imageData.Length; i+=3)
        {
            imageData[i] = i/3%10==0 ? (byte)255 : (byte)0;
            imageData[i+1] = 0;
            imageData[i+2] = 0;
        }
        MagickReadSettings mrs = new MagickReadSettings();
        mrs.Format = MagickFormat.Rgb;
        mrs.Width = 50;
        mrs.Height = 50;
        MagickImage mImage = new MagickImage(imageData, mrs);
        mImage.Write(tmpDir + "/image.png", MagickFormat.Png);

        Image image = new Image(tmpDir + "/image.png");
        image.Pixbuf = image.Pixbuf.ScaleSimple(800, 800, Gdk.InterpType.Tiles);
        _window.Add(image);
    }
}