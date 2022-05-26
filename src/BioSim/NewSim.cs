using Gtk;

namespace BioSim;

class NewSimButton : Button
{
    private SimulationDisplay _display;

    public NewSimButton(SimulationDisplay display) : base("New Simulation")
    {
        Clicked += OnButtenClicked;
        _display = display;
    }

    private void OnButtenClicked(object? sender, EventArgs e)
    {
        new NewSimWindow(_display);
    }
}


class NewSimWindow : Window
{
    private SimulationDisplay _display;
    public readonly Button createButton;
    public Simulation? Simulation { get; private set; }

    public NewSimWindow(SimulationDisplay display) : base("Create new Simulation")
    {
        _display = display;

        VBox outer = new VBox();
        Add(outer);

        // Simulation sim = new Simulation();

        HBox buttonBox = new HBox();
        outer.Add(buttonBox);

        createButton = new Button("Create");
        createButton.Clicked += CreateButtonClicked;
        buttonBox.Add(createButton);

        Button cancelButten = new Button("Cancel");
        cancelButten.Clicked += (object? sender, EventArgs e) => Destroy();
        buttonBox.Add(cancelButten);

        ShowAll();
    }

    private void CreateButtonClicked(object? obj, EventArgs e)
    {
        ReflectParameters();
        Destroy();
    }


    private void ReflectParameters()
    {
        System.Console.WriteLine("Hello");
        var numAttrFields = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            from field in type.GetFields()
                            where field.IsDefined(typeof(NumericalSimulationParameterAttribute), false)
                            select field;

        var typeAttrTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type.IsDefined(typeof(TypeSimulationParameterAttribute), false)
                            select type;

        var ass = AppDomain.CurrentDomain.GetAssemblies();
        var sim = typeof(Simulation);
        foreach (var a in ass)
        {
            foreach (var item in a.GetTypes())
            {
                foreach (var field in item.GetFields())
                {
                    if (field.IsDefined(typeof(NumericalSimulationParameterAttribute), true))
                    {
                        System.Console.WriteLine(field.Name);
                    }
                }
            }
        }
        foreach (var item in numAttrFields)
        {
            System.Console.WriteLine(item);
        }

        foreach (var item in typeAttrTypes)
        {
            System.Console.WriteLine(item);
        }

    }
}
