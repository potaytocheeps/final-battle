/// <summary>
/// Items that a character can equip to gain a special attack.
/// </summary>
public class Gear
{
    public string Name { get; }
    public Attack AttackProvided { get; }

    public Gear(string name, SpecialAttack attackProvided)
    {
        Name = name;
        AttackProvided = attackProvided;
    }

    public override string ToString() => Name.ToUpper();
}
