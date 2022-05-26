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

    private void OnButtenClicked(object sender, EventArgs e)
    {
        new NewSimWindow(_display);
    }
}


class NewSimWindow : Window
{
    private SimulationDisplay _display;
    public readonly Button createButton;
    public Simulation Simulation { get; private set; }

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
        cancelButten.Clicked += (object sender, EventArgs e) => Destroy();
        buttonBox.Add(cancelButten);

        ShowAll();
    }

    private void CreateButtonClicked(object obj, EventArgs e)
    {
        ReflectParameters();
        Destroy();
    }


    private void ReflectParameters()
    {
        var paraFields = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         from field in type.GetFields()
                         where field.IsDefined(typeof(SimulationParameterAttribute), false) |
                             (type.IsDefined(typeof(IncludeAllAsParametersAttribute), false) &
                             !field.IsDefined(typeof(ExcludeParameterAttribute), false))
                         select field;

        var paraProps = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        from prop in type.GetProperties()
                        where prop.IsDefined(typeof(SimulationParameterAttribute), false) |
                            (type.IsDefined(typeof(IncludeAllAsParametersAttribute), false) &
                            !prop.IsDefined(typeof(ExcludeParameterAttribute), false))
                        select prop;

        foreach (var item in paraFields)
        {
            System.Console.WriteLine(item);
        }

        foreach (var item in paraProps)
        {
            System.Console.WriteLine(item);
        }
    }
}
