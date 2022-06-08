using Xunit;
using BioSim;
using System.IO;
using ImageMagick;

namespace biosimtests;

// test the simulation class
public class SimulationTest
{
    private const string _mapImagefn = @"..\..\..\..\..\res\testres\small.png";

    // test construction
    [Fact]
    public void TestConstuct()
    {
        SimulationSettings settings = new SimulationSettings();
        settings.mapPath = _mapImagefn;
        Simulation simulation = new Simulation(settings);
    }

    // test update method
    [Fact]
    public void TestUpdate()
    {
        const int generations = 6;
        const int steps = 7;

        SimulationSettings settings = new SimulationSettings();
        settings.generations = generations;
        settings.steps = steps;
        settings.mapPath = _mapImagefn;
        Simulation simulation = new Simulation(settings);

        for (int i = 0; i < generations * steps; i++)
        {
            bool b = simulation.Update();
            if (simulation.CurrentState == SimulationState.Extinct)
            {
                break;
            }
            Assert.True(b);
        }

        Assert.False(simulation.Update());
    }

    // test a whole simulation walkthrough
    [Fact]
    public void TestWholeSim()
    {
        const string path = "../../../../../res/testres/corner.png";
        if (!File.Exists(path))
        {
            return;
        }

        MagickImage image = new MagickImage(path);

        InputFunction[] inputFunctions = { InputFunctions.NearToEast, InputFunctions.NearToSouth };
        OutputFunction[] outputFunctions = {OutputFunctions.MoveNorth, OutputFunctions.MoveSouth,
                                            OutputFunctions.MoveWest, OutputFunctions.MoveEast};

        SimulationSettings settings = new SimulationSettings();
        settings.initialPopulation = 1000;
        settings.generations = 20;
        settings.steps = 380;
        settings.minBirthAmount = 2;
        settings.maxBirthAmount = 10;
        settings.seed = 0;
        settings.inputFunctions = inputFunctions;
        settings.outputFunctions = outputFunctions;
        settings.mapPath = path;
        settings.connectionCount = 20;
        settings.innerCount = 5;


        Simulation simulation = new Simulation(settings);

        MagickImageCollection collection = new MagickImageCollection();
        MagickReadSettings mrs = new MagickReadSettings();
        mrs.Format = MagickFormat.Rgb;
        mrs.Width = image.Width;
        mrs.Height = image.Height;

        int counter = 0;
        while (simulation.Update())
        {
            collection.Add(new MagickImage(simulation.SimEnv.ReadData(), mrs));
            collection[counter].AnimationDelay = 5;
            counter++;
        }

        collection.Write("../../../../../tmp/firstsim.gif", MagickFormat.Gif);
    }
}
