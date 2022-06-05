namespace BioSim;

// holds map and dits
public class SimulationEnviroment
{
    public Map SimMap { get; set; }
    public List<Dit> Dits { get; set; }
    private Random _rnd;

    public Random RandomNumberGenerator
    {
        get
        {
            if (_rnd == null)
            {
                _rnd = new Random();
            }

            return _rnd;
        }
        set { _rnd = value; }
    }

    // constructor
    public SimulationEnviroment(Map simMap)
    {
        SimMap = simMap;
        Dits = new List<Dit>();
    }

    // constructor
    public SimulationEnviroment()
    {
        Dits = new List<Dit>();
    }

    // try adding amount dits
    public bool TryAddRandomDits(int amount, Model model)
    {
        List<(int, int)> validPositions = new List<(int, int)>();
        for (int y = 0; y < SimMap.Height; y++)
        {
            for (int x = 0; x < SimMap.Width; x++)
            {
                Map.CellType cell = SimMap.GetSpot(x, y);
                if (cell == Map.CellType.survive || cell == Map.CellType.space)
                {
                    validPositions.Add((x, y));
                }
            }
        }

        int count = Math.Min(amount, validPositions.Count);
        bool empty = count == validPositions.Count;
        for (int i = 0; i < count; i++)
        {
            int index = RandomNumberGenerator.Next(validPositions.Count);

            Model cModel = model.Copy();
            cModel.Randomize();

            Dit dit = new Dit(validPositions[index], cModel);
            Dits.Add(dit);
            validPositions.RemoveAt(index);
        }

        return empty;
    }

    // try add amount dits with specific model
    public bool TryAddRandomDits(List<(int amount, Model model)> toAdd)
    {
        List<(int, int)> validPositions = new List<(int, int)>();
        for (int y = 0; y < SimMap.Height; y++)
        {
            for (int x = 0; x < SimMap.Width; x++)
            {
                Map.CellType cell = SimMap.GetSpot(x, y);
                if (cell == Map.CellType.survive || cell == Map.CellType.space)
                {
                    validPositions.Add((x, y));
                }
            }
        }

        bool empty = false;
        foreach (var item in toAdd)
        {
            int count = Math.Min(item.amount, validPositions.Count);
            empty = count == validPositions.Count;
            for (int i = 0; i < count; i++)
            {
                int index = RandomNumberGenerator.Next(validPositions.Count);

                Model cModel = item.model.Copy();
                cModel.Randomize();
                cModel.Mutate();

                Dit dit = new Dit(validPositions[index], cModel);
                Dits.Add(dit);

                validPositions.RemoveAt(index);
            }

            if (empty)
            {
                break;
            }
        }

        return empty;
    }

    // try move to new position
    public bool TryMove(Dit dit, (int x, int y) newPosition)
    {
        if (newPosition.x >= 0 && newPosition.x < SimMap.Width &&
            newPosition.y >= 0 && newPosition.y < SimMap.Height)
        {
            foreach (var item in Dits)
            {
                if (item.Position == newPosition)
                {
                    return false;
                }
            }

            Map.CellType cell = SimMap.GetSpot(newPosition);
            if ((cell == Map.CellType.survive || cell == Map.CellType.space))
            {
                dit.Position = newPosition;
                return true;
            }
        }
        return false;
    }

    // kill dits and create new ones
    public void KillAndCreateDits(int minBirthAmount, int maxBirthAmount)
    {
        List<Dit> oldDits = Dits.FindAll((Dit dit) => SimMap.GetSpot(dit.Position) == Map.CellType.survive);
        List<(int, Model)> toAdd = new List<(int, Model)>();
        Dits.Clear();

        while (oldDits.Count != 0)
        {
            int amount = RandomNumberGenerator.Next(minBirthAmount, maxBirthAmount + 1);
            int index = RandomNumberGenerator.Next(oldDits.Count);
            toAdd.Add((amount, oldDits[index].Model));
            oldDits.RemoveAt(index);
        }

        TryAddRandomDits(toAdd);
    }

    // get map and dit data
    public byte[] ReadData()
    {
        byte[] data = new byte[SimMap.RawData.Length];
        SimMap.RawData.CopyTo(data, 0);

        foreach (var item in Dits)
        {
            int index = (item.Position.x + item.Position.y * SimMap.Width) * 3;
            data[index] = 255;
            data[index + 1] = 0;
            data[index + 2] = 0;
        }

        return data;
    }
}
