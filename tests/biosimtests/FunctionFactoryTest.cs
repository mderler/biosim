using BioSim;
using Xunit;

namespace biosimtests;

// test the FunctionFactory class
public class FunctionFactoryTest
{
    // test adding fuctions
    [Fact]
    public void TestAddFunctions()
    {
        FunctionFactory.RegisterIOFunction("func1", InputFunctions.NearToEast);
        FunctionFactory.RegisterIOFunction("func2", OutputFunctions.MoveEast);
        FunctionFactory.RegisterIOFunction("func2", InputFunctions.NearToSouth);
        FunctionFactory.RegisterIOFunction("func2", OutputFunctions.MoveNorth);
    }

    // test getting functions
    [Fact]
    public void GetFunctions()
    {
        FunctionFactory.RegisterIOFunction("func1", InputFunctions.NearToEast);
        FunctionFactory.RegisterIOFunction("func2", OutputFunctions.MoveEast);
        FunctionFactory.RegisterIOFunction("func2", InputFunctions.NearToSouth);
        FunctionFactory.RegisterIOFunction("func2", OutputFunctions.MoveNorth);

        Assert.Equal(2, FunctionFactory.RegisterdInputFunctions.Count);
        Assert.Equal(1, FunctionFactory.RegisterdOutputFunctions.Count);
    }
}
