/// <summary>
/// Defensive attack modifier that gives a character a 1-point damage reduction
/// from any incoming attack.
/// </summary>
public class StoneArmor: Modifier
{
    public StoneArmor() : base("Stone Armor", damageAmount: 1, ModifierType.Defensive) { }
}
