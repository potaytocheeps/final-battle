/// <summary>
/// The final boss of the game.
/// </summary>
public class TheUncodedOne : Character
{
    public TheUncodedOne() : base("The Uncoded One", maxHP: 120)
    {
        _attacks.Insert(0, new Unraveling());
    }
}
