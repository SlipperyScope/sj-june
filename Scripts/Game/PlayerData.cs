using Godot;
using System;
using System.Collections.Generic;

public enum SteveID
{
    Scuba
}

public class PlayerData : Node
{
    public Dictionary<SteveID, String> StevePaths = new Dictionary<SteveID, string>()
    {
        { SteveID.Scuba, "res://Game/Characters/ScubaSteve.tscn" }
    };
    String textHistory;
    List<String> itemNames = new List<String>();
    public override void _Ready()
    {
        textHistory = "";
    }

    public String getTextHistory()
    {
        return textHistory;
    }

    public void prependTextHistory(String newText)
    {
        textHistory = newText + "\n" + textHistory;
    }

    public List<String> getItemNames()
    {
        return itemNames;
    }

    public void addItem(String item)
    {
        itemNames.Add(item);
    }

    public void removeItem(String item)
    {
        itemNames.Remove(item);
    }
}
