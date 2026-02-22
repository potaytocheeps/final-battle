/// <summary>
/// Equipable item that provides the character that equips it with the
/// special attack: Slash
/// </summary>
public class Sword : Gear
{
    public Sword(DamageType damageType = DamageType.Physical) : base("Sword", new Slash(damageType))
    {
        if (damageType != DamageType.Physical) Name = $"{damageType} Sword";
    }
}
