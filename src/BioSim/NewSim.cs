using System;
using Gtk;

class NewSimButton : Button
{
    public NewSimButton() : base("New Simulation")
    {
        Clicked += delegate { new NewSimWindow(); };
    }
}


class NewSimWindow : Window
{
    public NewSimWindow() : base("Create new Simulation")
    {
        VBox outer = new VBox();
        Add(outer);

        HBox simNameBox = new HBox();
        outer.Add(simNameBox);
        Label simNameLabel = new Label("Simulation Name: ");
        simNameBox.Add(simNameLabel);
        Entry simNameEntry = new Entry();
        simNameBox.Add(simNameEntry);

        HBox worldSizeBox = new HBox();
        outer.Add(worldSizeBox);
        Label worldSizeLabel = new Label("World Size");
        worldSizeBox.Add(worldSizeLabel);
        Label worldXLabel = new Label("x:");
        worldSizeBox.Add(worldXLabel);
        Entry worldXEntry = new Entry();
        worldSizeBox.Add(worldXEntry);
        Label worldYLabel = new Label("y:");
        worldSizeBox.Add(worldYLabel);
        Entry worldYEntry = new Entry();
        worldSizeBox.Add(worldYEntry);

        HBox buttonBox = new HBox();
        outer.Add(buttonBox);

        Button createButton = new Button("Create");
        createButton.Clicked += OnCreateButtonClicked;
        buttonBox.Add(createButton);

        Button cancelButten = new Button("Cancel");
        cancelButten.Clicked += (object? sender, EventArgs e) => Destroy();
        buttonBox.Add(cancelButten);

        ShowAll();
    }

    private void OnCreateButtonClicked(object? sender, EventArgs e)
    {
        
    }
}