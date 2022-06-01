namespace BioSim;

// base class for Models
// the brain of a Dit
public abstract class Model
{
    public float MutateChance { get; set; }
    public float MutateStrength { get; set; }
    public Random RNG { get; set; }

    // constructor
    public Model(float mutateChance, float mutateStrength, Random rng)
    {
        RNG = rng == null ? new Random() : rng;
        MutateChance = mutateChance;
        MutateStrength = mutateStrength;
    }

    // constructor
    public Model(float mutateChance, float mutateStrength)
    {
        RNG = new Random();
        MutateChance = mutateChance;
        MutateStrength = mutateStrength;
    }

    // constructor
    public Model()
    {
        RNG = new Random();
    }

    public abstract bool[] GetOutput(float[] input);
    public abstract void Randomize();
    public abstract void Mutate();
    public abstract Model Copy();
}
