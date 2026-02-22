/// <summary>
/// Equipable item that provides the character that equips it with the
/// special attack: Stab
/// </summary>
public class Dagger : Gear
{
    public Dagger(DamageType damageType = DamageType.Physical) : base("Dagger", new Stab(damageType))
    {
        if (damageType != DamageType.Physical) Name = $"{damageType} Dagger";
    }
}
