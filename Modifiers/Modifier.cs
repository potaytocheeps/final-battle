/// <summary>
/// Modifiers provide defensive capabilities to characters, which allow them
/// to reduce the damage that they take from attacks.
/// </summary>
public class Modifier(string name, int damageAmount, ModifierType modifierType)
{
    private readonly string _name = name;
    private readonly int _damageAmount = damageAmount;
    private ModifierType _modifierType = modifierType;

    public virtual int CalculateModifiedDamage(int damage, DamageType attackDamageType)
    {
        int modifiedDamage;

        if (_modifierType == ModifierType.Defensive)
        {
            ColoredConsole.WriteLine($"{this} reduced the attack by {_damageAmount} damage.");

            modifiedDamage = damage - _damageAmount;

            return Math.Max(0, modifiedDamage);
        }
        else if (_modifierType == ModifierType.Offensive)
        {
            ColoredConsole.WriteLine($"{this} increased the attack by {_damageAmount} damage.");

            modifiedDamage = damage + _damageAmount;

            return modifiedDamage;
        }

        return damage;
    }

    public override string ToString() => _name.ToUpper();
}


// Defines the different types of modifiers that a character can have
public enum ModifierType { Defensive, Offensive }
