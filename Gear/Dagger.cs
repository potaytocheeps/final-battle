/// <summary>
/// Equipable item that provides the character that equips it with the
/// special attack: Stab
/// </summary>
public class Dagger : Gear
{
    public Dagger(DamageType damageType = DamageType.Normal) : base($"{damageType} Dagger", new Stab(damageType)) { }
}
