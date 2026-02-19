/// <summary>
/// Equipable item that provides the character that equips it with the
/// special attack: Slash
/// </summary>
public class Sword : Gear
{
    public Sword(DamageType damageType = DamageType.Normal) : base($"{damageType} Sword", new Slash(damageType)) { }
}
