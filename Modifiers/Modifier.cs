/// <summary>
/// Modifiers provide defensive capabilities to characters, which allow them
/// to reduce the damage that they take from attacks.
/// </summary>
public class Modifier(string name, int damageReductionAmount, ModifierType modifierType)
{
    private readonly string _name = name;
    private readonly int _damageReductionAmount = damageReductionAmount;
    public ModifierType ModifierType { get; } = modifierType;

    public int CalculateModifiedDamage(int damage)
    {
        if (ModifierType == ModifierType.Defensive)
        {
            ColoredConsole.WriteLine($"{this} reduced the attack by {_damageReductionAmount} point.");

            int modifiedDamage = damage - _damageReductionAmount;

            return modifiedDamage <= 0 ? 0 : modifiedDamage;
        }

        return damage;
    }

    public override string ToString() => _name.ToUpper();
}


// Defines the different types of modifiers that a character can have
public enum ModifierType { Defensive }
