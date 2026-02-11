/// <summary>
/// Enemy character that has a defensive attack modifier, reducing all incoming
/// damage by 1 point.
/// </summary>
public class StoneAmarok : Character
{
    public StoneAmarok(Gear? startingGear = null) : base("Stone Amarok", maxHP: 4, startingGear)
    {
        _attacks.Insert(0, new Bite());
        _modifiers.Add(ModifierType.Defensive, [new StoneArmor()]);
    }

    public StoneAmarok(int number, Gear? startingGear = null) : base($"Stone Amarok {number}", maxHP: 4, startingGear)
    {
        _attacks.Insert(0, new Bite());
        _modifiers.Add(ModifierType.Defensive, [new StoneArmor()]);
    }
}
