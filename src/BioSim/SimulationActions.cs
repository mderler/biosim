using ImageMagick;

namespace BioSim;

public class VisualizeAction : SimulationAction
{
    private MagickImageCollection _collection = new MagickImageCollection();
    private MagickReadSettings _mrs;
    private int _frameTime;

    public VisualizeAction(string actionString) : base(actionString)
    {
        char[] sarr = { '/', '\\' };

        int index = _args[0].LastIndexOfAny(sarr);
        string path = _args[0].Remove(index);

        if (!Directory.Exists(path))
        {
            Error += $"the path does not exist {path}\n";
        }
        if (Directory.Exists(_args[0]))
        {
            Error += $"{_args[0]} is a directory not a file\n";
        }

        if (!int.TryParse(_args[1], out _frameTime))
        {
            Error += "second arg must be an integer\n";
        }
    }

    protected override void Start()
    {
        _mrs = new MagickReadSettings();
        _mrs.Format = MagickFormat.Rgb;
        _mrs.Width = Simulation.SimMap.Width;
        _mrs.Height = Simulation.SimMap.Height;
    }

    private int _counter = 0;
    protected override void Execute()
    {
        _collection.Add(new MagickImage(Simulation.SimEnv.ReadData(), _mrs));
        _collection[_counter].AnimationDelay = _frameTime;
        _counter++;
    }

    protected override void End()
    {
        _collection.Write(_args[0], MagickFormat.Gif);
    }

    public override SimulationAction Copy()
    {
        return new VisualizeAction(_actionString);
    }
}
