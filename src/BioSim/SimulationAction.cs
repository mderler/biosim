namespace BioSim;

public abstract class SimulationAction
{
    protected string[] _args;
    public Simulation Simulation { get; set; }
    protected string _actionString;
    protected ArgsChecker _checker = new ArgsChecker();

    public string Error { get; protected set; }

    private (uint gf, uint gl, uint sf, uint sl)[] _executeTimes;

    public string Setup(string eventString, string[] args)
    {
        _actionString = eventString;

        string[] allTimes = eventString.Split(';');
        _executeTimes = new (uint, uint, uint, uint)[allTimes.Length];

        for (int i = 0; i < allTimes.Length; i++)
        {
            uint[] times = new uint[4];
            string[] tmp = allTimes[i].Split(',');

            if (tmp.Length != 4)
            {
                Error += "error reading times";
                return Error;
            }

            bool mistake = false;

            for (int j = 0; j < 4; j++)
            {
                if (tmp[j] == "s")
                {
                    times[j] = 0;
                    continue;
                }
                if (tmp[j] == "b")
                {
                    times[j] = uint.MaxValue;
                    continue;
                }
                mistake = !uint.TryParse(tmp[j], out times[j]);
                if (mistake)
                {
                    break;
                }
            }

            if (mistake)
            {
                Error += "error reading times";
                return Error;
            }

            _executeTimes[i] = (times[0], times[1], times[2], times[3]);
        }

        string error;
        if (!_checker.Check(args, out error))
        {
            Error += error;
            return Error;
        }

        return PSetup();
    }

    protected virtual string PSetup() { return ""; }

    public void ExecuteAction()
    {

    }

    protected virtual void Start() { }
    protected virtual void Execute() { }
    protected virtual void End() { }

    public abstract SimulationAction Copy();
}
