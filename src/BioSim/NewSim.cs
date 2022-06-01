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
        bool noerror = true;
        int val;
        float fval;
        noerror |= int.TryParse(_settings["min_birth_amount"].Text, out val);
        _sim.MinBirthAmount = val;
        noerror |= int.TryParse(_settings["max_birth_amount"].Text, out val);
        _sim.MaxBirthAmount = val;
        noerror |= int.TryParse(_settings["initial_population"].Text, out val);
        _sim.InitialPopulation = val;
        noerror |= int.TryParse(_settings["steps"].Text, out val);
        _sim.Steps = val;
        noerror |= int.TryParse(_settings["generations"].Text, out val);
        _sim.Generations = val;
        noerror |= int.TryParse(_settings["seed"].Text, out val);
        rnd = new Random(val);
        noerror |= File.Exists(_settings["map_path"].Text);
        if (noerror)
        {
            map = new Map(_settings["map_path"].Text);
            _sim.SimMap = map;
        }
        noerror |= float.TryParse(_settings["mutate_chance"].Text, out fval);
        model.MutateChance = fval;
        noerror |= float.TryParse(_settings["mutate_strength"].Text, out fval);
        model.MutateStrength = fval;
        if (!noerror)
        {
            return;
        }
        _sim.RandomNumberGenerator = rnd;
        model.RNG = rnd;
        Destroy();
    }
}
