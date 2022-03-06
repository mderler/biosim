namespace BioSim;

public delegate float InputFunction(Dit dit);

public static class InputFunctions
{
    public static float GetOneTest(Dit dit)
    {
        return 1;
    }
}