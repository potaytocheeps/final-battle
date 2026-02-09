/// <summary>
/// The most basic enemy character in the game.
/// </summary>
public class Skeleton : Character
{
    public Skeleton(Gear? startingGear = null) : base("Skeleton", maxHP: 5, startingGear)
    {
        _attacks.Insert(0, new BoneCrunch());
    }

    public Skeleton(int number, Gear? startingGear = null) : base($"Skeleton {number}", maxHP: 5, startingGear)
    {
        _attacks.Insert(0, new BoneCrunch());
    }
}
