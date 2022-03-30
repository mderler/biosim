namespace BioSim;

public class SimulationEnviroment
{ 
    public Map SimMap { get; set; }
    public Random RandomNumberGenerator { get; set; }
    public List<Dit> Dits { get; private set; }

    public SimulationEnviroment(Map simMap)
    {
        RandomNumberGenerator = new Random();
        SimMap = simMap;
        Dits = new List<Dit>();
    }


    public bool TryAddRandomDits(int amount, Model model, bool mutate=false)
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
            if (mutate)
            {
                cModel.Mutate();
            }
            Dit dit = new Dit(validPositions[index], cModel);
            Dits.Add(dit);
            validPositions.RemoveAt(index);
        }

        return empty;
    }

    public bool TryMove(Dit dit, (int x, int y) newPosition)
    {
        Map.CellType cell = SimMap.GetSpot(newPosition);
        if (cell == Map.CellType.survive || cell == Map.CellType.space)
        {
            dit.position = newPosition;
            return true;
        }
        return false;
    }

    public void KillAndCreateDits(int minBirthAmount, int maxBirthAmount)
    {
        List<Dit> oldDits = Dits.FindAll((Dit dit) => SimMap.GetSpot(dit.position) == Map.CellType.survive);
        Dits.Clear();

        // increment to overcome the problem of the number generator where the upper end ist exluded.
        maxBirthAmount++;
        bool running = true;
        while (oldDits.Count != 0 && running)
        {
            int amount = RandomNumberGenerator.Next(minBirthAmount, maxBirthAmount);
            int index = RandomNumberGenerator.Next(oldDits.Count);
            running = TryAddRandomDits(amount,oldDits[index].model, true);
        }
    }

    public byte[] ReadData()
    {
        byte[] data = SimMap.ReadData();

        foreach (var item in Dits)
        {
            int index = (item.position.x+item.position.y*SimMap.Width)*3;
            data[index] = 255;
            data[index+1] = 0;
            data[index+2] = 0;
        }

        return data;
    }
}
