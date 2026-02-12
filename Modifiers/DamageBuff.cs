/// <summary>
/// Offensive attack modifier that gives a character a 1-point damage increase
/// for any attack.
/// </summary>
public class DamageBuff: Modifier
{
    public DamageBuff() : base("Damage Buff", damageAmount: 1, ModifierType.Offensive) { }
}
