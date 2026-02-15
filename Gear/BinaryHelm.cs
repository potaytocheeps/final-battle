/// <summary>
/// Equipable item that provides the character that equips it with the
/// attack modifiers: Damage Buff and Stone Armor
/// </summary>
public class BinaryHelm : Gear
{
    public BinaryHelm() : base("Binary Helm")
    {
        Modifiers = [new DamageBuff(), new StoneArmor()];
    }
}
