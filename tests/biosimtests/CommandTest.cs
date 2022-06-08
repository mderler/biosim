using BioSim;
using Xunit;
namespace biosimtests;

// test the Command class
public class CommandTest
{
    // a test command used for the tests
    private class TestCommand : Command
    {
        // constructor
        public TestCommand(BioSimulator simulator) : base(simulator)
        {
            _checker.MinArgs = 2;
            _checker.BannendWords.Add("test");
            _checker.BannendWords.Add("hi");
        }

        // run command
        protected override string Run(string[] args)
        {
            return "t";
        }
    }

    // Test Construction
    [Fact]
    public void TestConstuct()
    {
        BioSimulator bioSimulator = new BioSimulator();
        TestCommand command = new TestCommand(bioSimulator);
    }

    // test the min args check
    [Fact]
    public void TestMinArgs()
    {
        BioSimulator bioSimulator = new BioSimulator();
        TestCommand command = new TestCommand(bioSimulator);

        string[] args = { "asd", "qwe" };

        Assert.Equal("t", command.RunCommand(args));

        args = new string[1];
        args[0] = "asd";

        Assert.Equal("1 argument(s) are missing", command.RunCommand(args));
    }

    // test the bannend words check
    [Fact]
    public void TestBannendWords()
    {
        BioSimulator bioSimulator = new BioSimulator();
        TestCommand command = new TestCommand(bioSimulator);

        string[] args = { "test", "hi" };

        Assert.Equal("the expression 'test' is preserved", command.RunCommand(args));

        args[0] = "hi";
        args[1] = "test";

        Assert.Equal("the expression 'hi' is preserved", command.RunCommand(args));
    }

    // test the empty or null check
    [Fact]
    public void TestEmpty()
    {
        BioSimulator bioSimulator = new BioSimulator();
        TestCommand command = new TestCommand(bioSimulator);

        string[] args = { "", "asd" };

        Assert.Equal("the args must not be empty", command.RunCommand(args));
    }

    // test a runthrough with correct arguments
    [Fact]
    public void TestCorrect()
    {
        BioSimulator bioSimulator = new BioSimulator();
        TestCommand command = new TestCommand(bioSimulator);

        string[] args = { "hello", "mom" };

        Assert.Equal("t", command.RunCommand(args));
    }
}
