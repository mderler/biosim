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
                            where field.IsDefined(typeof(NumericalSimulationParameterAttribute), true)
                            select field;

        var numAttrProps = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           from prop in type.GetProperties()
                           where prop.IsDefined(typeof(NumericalSimulationParameterAttribute), true)
                           select prop;

        var typeAttrTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type.IsDefined(typeof(TypeSimulationParameterAttribute), true)
                            select type;

        foreach (var item in numAttrFields)
        {
            System.Console.WriteLine(item.Name);
        }

        foreach (var item in numAttrProps)
        {
            System.Console.WriteLine(item.Name);
        }

        foreach (var item in typeAttrTypes)
        {
            System.Console.WriteLine(item.Name);
        }

    }
}
