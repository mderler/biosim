using Gtk;
using ImageMagick;

class BioSimulator
{
    private Window _window;

    public BioSimulator()
    {
        _window = new Window("BioSim");
        _window.SetDefaultSize(1270, 720);
        _window.DeleteEvent += OnWindowDelete;

        Frame outerFrame = new Frame();
        _window.Add(outerFrame);

        HBox outerHBox = new HBox();
        outerFrame.Add(outerHBox);
        
        VBox simulationList = new VBox();
        simulationList.Add(new Label("This is the simulation list"));
        simulationList.Add(new Label("This is the simulation list"));
        simulationList.Add(new Label("This is the simulation list"));
        simulationList.Add(new Label("This is the simulation list"));
        simulationList.Add(new Label("This is the simulation list"));
        outerHBox.Add(simulationList);

        VBox controlPanel = new VBox();
        controlPanel.Add(new Label("This is the control panel"));
        controlPanel.Add(new Label("This is the control panel"));
        controlPanel.Add(new Label("This is the control panel"));
        controlPanel.Add(new Label("This is the control panel"));
        controlPanel.Add(new Label("This is the control panel"));
        outerHBox.Add(controlPanel);
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

    private void OnWindowDelete(object sender, DeleteEventArgs e)
    {
        Application.Quit();
    }
}