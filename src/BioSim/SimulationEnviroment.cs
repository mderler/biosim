namespace BioSim;

class SimulationEnviroment
{ 
    public Map SimMap { get; set; }
    public Random RandomNumberGenerator { get; set; }
    private List<Dit> _dits;

    public SimulationEnviroment(Map simMap)
    {
        RandomNumberGenerator = new Random();
        SimMap = simMap;
        _dits = new List<Dit>();
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
        List<Dit> oldDits = _dits.FindAll((Dit dit) => SimMap.GetSpot(dit.position) == Map.CellType.survive);
        _dits.Clear();

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
}
