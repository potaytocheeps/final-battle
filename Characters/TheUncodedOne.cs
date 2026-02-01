/// <summary>
/// The final boss of the game.
/// </summary>
public class TheUncodedOne : Character
{
    public TheUncodedOne() : base("The Uncoded One", maxHP: 15)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Unraveling"));
    }
}
