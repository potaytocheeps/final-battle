/// <summary>
/// The final boss of the game.
/// </summary>
public class TheUncodedOne : Character
{
    public TheUncodedOne(int maxHP) : base("The Uncoded One", maxHP)
    {
        _attacks.Insert(0, new Unraveling());
    }
}
