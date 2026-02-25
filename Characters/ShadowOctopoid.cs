/// <summary>
/// Enemy character that has a chance to steal gear from its attack target.
/// </summary>
public class ShadowOctopoid : Character
{
    public ShadowOctopoid(Gear? startingGear, Modifier? startingModifier, int maxHP) : base("Shadow Octopoid", startingGear, startingModifier, maxHP)
    {
        _attacks.Insert(0, new Grapple());
    }
}
