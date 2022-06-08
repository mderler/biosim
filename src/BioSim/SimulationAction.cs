namespace BioSim;

public abstract class SimulationAction
{
    protected string[] _args;
    public Simulation Simulation { get; set; }
    protected string _actionString;

    public string Error { get; protected set; }

    private (uint gf, uint gl, uint sf, uint sl)[] _executeTimes;

    public SimulationAction(string eventString)
    {
        _actionString = eventString;

        string[] allTimes = eventString.Split(';');
        _executeTimes = new (uint, uint, uint, uint)[allTimes.Length];

        for (int i = 0; i < allTimes.Length; i++)
        {
            uint[] times = new uint[4];
            string[] tmp = allTimes[i].Split(',');

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
                return;
            }

            _executeTimes[i] = (times[0], times[1], times[2], times[3]);
        }

    }

    public void ExecuteAction()
    {

    }

    protected virtual void Start() { }
    protected virtual void Execute() { }
    protected virtual void End() { }

    public abstract SimulationAction Copy();
}
