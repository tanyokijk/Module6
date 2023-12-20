namespace Models;

public class Game
{
    public enum Mode
    {
        SinglePlayer,
        Multiplayer,
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Studio { get; set; }

    public string Style { get; set; }

    public DateOnly DateRelease { get; set; }

    public Mode GameplayMode { get; set; }

    public int NumberSold { get; set; }

    public Game(string name, string studio, string style, DateOnly dateRelease, Mode gameplayMode, int numberSold)
    {
        this.Name = name;
        this.Studio = studio;
        this.Style = style;
        this.DateRelease = dateRelease;
        this.GameplayMode = gameplayMode;
        this.NumberSold = numberSold;
    }
}