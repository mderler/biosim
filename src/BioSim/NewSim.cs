// creates a new window that creates a new simulation when clicked

using Gtk;

namespace BioSim;

// when clicked "NewSimWindow"
class NewSimButton : Button
{
    private SimulationDisplay _display;

    // constructor
    public NewSimButton(SimulationDisplay display) : base("New Simulation")
    {
        Clicked += OnButtenClicked;
        _display = display;
    }

    // create Window
    private void OnButtenClicked(object sender, EventArgs e)
    {
        new NewSimWindow(_display);
    }
}

// allow to make settings
class NewSimWindow : Window
{
    private Simulation _sim;
    private SimulationDisplay _display;
    public readonly Button createButton;
    public Simulation Simulation { get; private set; }

    private Dictionary<string, Entry> _settings = new Dictionary<string, Entry>();

    // constructor
    public NewSimWindow(SimulationDisplay display) : base("Create new Simulation")
    {
        # region Set Settings Entries
        _settings.Add("generations", null);
        _settings.Add("steps", null);
        _settings.Add("min_birth_amount", null);
        _settings.Add("max_birth_amount", null);
        _settings.Add("initial_population", null);
        _settings.Add("map_path", null);
        _settings.Add("seed", null);
        _settings.Add("mutate_chance", null);
        _settings.Add("mutate_strength", null);
        _settings.Add("inner_count", null);

        foreach (var item in _settings.Keys)
        {
            _settings[item] = new Entry();
        }
        # endregion


        _display = display;

        VBox outer = new VBox();
        Add(outer);

        VBox settings = new VBox();
        outer.PackStart(settings, false, false, 4);

        HBox buttonBox = new HBox();
        outer.PackStart(buttonBox, false, false, 4);

        createButton = new Button("Create");
        createButton.Clicked += CreateButtonClicked;
        buttonBox.PackStart(createButton, true, true, 2);

        Button cancelButten = new Button("Cancel");
        cancelButten.Clicked += (object sender, EventArgs e) => Destroy();
        buttonBox.PackStart(cancelButten, true, true, 2);

        _sim = new Simulation();

        foreach (var item in _settings)
        {
            string name = item.Key.Replace('_', ' ');
            HBox temp = new HBox();
            temp.PackStart(new Label(name), false, false, 4);
            temp.PackStart(item.Value, false, false, 4);
            settings.PackStart(temp, false, false, 4);
        }

        ShowAll();
    }

    // create Simulation
    private void CreateButtonClicked(object obj, EventArgs e)
    {
        Map map;
        Random rnd;
        SLLModel model = new SLLModel();
        int val;
        float fval;
        if (!int.TryParse(_settings["min_birth_amount"].Text, out val))
        {
            return;
        }
        _sim.MinBirthAmount = val;
        if (!int.TryParse(_settings["max_birth_amount"].Text, out val))
        {
            return;
        }
        _sim.MaxBirthAmount = val;
        if (!int.TryParse(_settings["initial_population"].Text, out val))
        {
            return;
        }
        _sim.InitialPopulation = val;
        if (!int.TryParse(_settings["steps"].Text, out val))
        {
            return;
        }
        _sim.Steps = val;
        if (!int.TryParse(_settings["generations"].Text, out val))
        {
            return;
        }
        _sim.Generations = val;
        if (!int.TryParse(_settings["seed"].Text, out val))
        {
            return;
        }
        rnd = new Random(val);
        if (!File.Exists(_settings["map_path"].Text))
        {
            return;
        }
        map = new Map(_settings["map_path"].Text);
        _sim.SimMap = map;
        if (!float.TryParse(_settings["mutate_chance"].Text, out fval))
        {
            return;
        }
        model.MutateChance = fval;
        if (!float.TryParse(_settings["mutate_strength"].Text, out fval))
        {
            return;
        }
        model.MutateStrength = fval;
        if (!int.TryParse(_settings["inner_count"].Text, out val))
        {
            return;
        }
        model.InnerCount = val;

        _sim.RandomNumberGenerator = rnd;
        model.RNG = rnd;
        Destroy();
    }
}
