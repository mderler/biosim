using Gtk;

class NewSimButton : Button
{
    public NewSimButton() : base("New Simulation")
    {
        Clicked += OnButtenClicked;
    }

    private void OnButtenClicked(object? sender, EventArgs e)
    {
        new NewSimWindow();
    }
}


class NewSimWindow : Window
{
    private SimulationData _data;

    public NewSimWindow() : base("Create new Simulation")
    {
        VBox outer = new VBox();
        Add(outer);
        HBox buttonBox = new HBox();
        outer.Add(buttonBox);

        Button createButton = new Button("Create");
        createButton.Clicked += OnCreateButtonClicked;
        buttonBox.Add(createButton);

        Button cancelButten = new Button("Cancel");
        cancelButten.Clicked += (object? sender, EventArgs e) => Destroy();
        buttonBox.Add(cancelButten);

        Label simNameLabel = new Label("Simulation Name");
        Label worldSize = new Label("World Size");
        Label worldX = new Label("x");
        Label worldy = new Label("y");

        ShowAll();
    }

    private void OnCreateButtonClicked(object? sender, EventArgs e)
    {
        
    }
}