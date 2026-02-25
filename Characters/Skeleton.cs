/// <summary>
/// The most basic enemy character in the game.
/// </summary>
public class Skeleton : Character
{
    public Skeleton(Gear? startingGear, Modifier? startingModifier, int maxHP) : base("Skeleton", startingGear, startingModifier, maxHP)
    {
        _attacks.Insert(0, new BoneCrunch());
    }
}
