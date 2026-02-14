/// <summary>
/// Items that a character can equip to gain a special attack.
/// </summary>
public abstract class Gear
{
    public string Name { get; }
    public Attack AttackProvided { get; }
    protected List<Modifier> Modifiers { get; set; }
    public IReadOnlyList<Modifier> ModifiersProvided => Modifiers;
    public bool ProvidesModifiers => ModifiersProvided.Count > 0;

    public Gear(string name, SpecialAttack attackProvided)
    {
        Name = name;
        AttackProvided = attackProvided;
        Modifiers = [];
    }

    public override string ToString() => Name.ToUpper();
}
