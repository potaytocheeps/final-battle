/// <summary>
/// Enemy character that has a defensive attack modifier, reducing all incoming
/// damage by 1 point.
/// </summary>
public class StoneAmarok : Character
{
    public StoneAmarok(Gear? startingGear, Modifier? startingModifier, int maxHP) : base("Stone Amarok", startingGear, startingModifier, maxHP)
    {
        _attacks.Insert(0, new Bite());
    }
}
