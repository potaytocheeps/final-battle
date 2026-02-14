/// <summary>
/// Modifiers provide defensive capabilities to characters, which allow them
/// to reduce the damage that they take from attacks.
/// </summary>
public abstract class Modifier(string name, int damageAmount, ModifierType modifierType)
{
    private readonly string _name = name;
    private readonly int _damageAmount = damageAmount;
    public ModifierType ModifierType { get; private set; } = modifierType;

    public virtual int CalculateModifiedDamage(Character character, int damage, DamageType attackDamageType)
    {
        int modifiedDamage = damage;

        if (ModifierType == ModifierType.Defensive)
        {
            modifiedDamage = Math.Max(0, damage - _damageAmount);

            ColoredConsole.WriteLine($"{character}'s {this} reduced the attack by " +
                                     $"{_damageAmount} damage: ({damage}->{modifiedDamage})");
        }
        else if (ModifierType == ModifierType.Offensive)
        {
            modifiedDamage = damage + _damageAmount;

            ColoredConsole.WriteLine($"{character}'s {this} increased the attack by " +
                                     $"{_damageAmount} damage: ({damage}->{modifiedDamage})");
        }

        return modifiedDamage;
    }

    public override string ToString() => _name.ToUpper();
}


// Defines the different types of modifiers that a character can have
public enum ModifierType { Defensive, Offensive }
