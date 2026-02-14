/// <summary>
/// Defines characteristics of the different attacks that a character can
/// perform against enemy characters.
/// </summary>
public abstract class Attack
{
    public string Name { get; }
    public int Damage { get; protected set; }
    public AttackType AttackType { get; }
    public float HitChance { get; protected set; }
    private readonly DamageType _damageType;

    public Attack(string name, AttackType attackType, DamageType damageType)
    {
        Name = name;
        AttackType = attackType;
        HitChance = 1; // By default, an attack has a 100% chance of hitting its target
        _damageType = damageType;
    }

    public void DealDamage(Character user, Character attackTarget)
    {
        // The damage that an attack deals can vary per turn. It should be calculated
        // each time the attack is used during a turn
        int damageAmount = CalculateDamage();

        // Modify damage amount if attack target has any damage modifiers
        if (user.HasOffensiveDamageModifier)
        {
            List<Modifier> offensiveModifiers = user.Modifiers[ModifierType.Offensive];

            foreach (Modifier modifier in offensiveModifiers)
            {
                damageAmount = modifier.CalculateModifiedDamage(user, damageAmount, _damageType);
            }
        }

        if (attackTarget.HasDefensiveDamageModifier)
        {
            List<Modifier> defensiveModifiers = attackTarget.Modifiers[ModifierType.Defensive];

            foreach (Modifier modifier in defensiveModifiers)
            {
                damageAmount = modifier.CalculateModifiedDamage(attackTarget, damageAmount, _damageType);
            }
        }

        string damageType = _damageType.ToString().ToUpper();

        // Display results of having performed the attack
        attackTarget.TakeDamage(damageAmount);
        ColoredConsole.WriteLine($"{this} dealt {damageAmount} {damageType} damage to {attackTarget}.");

        if (attackTarget is MylaraAndSkorin) ColoredConsole.WriteLine($"{attackTarget} are now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");
        else ColoredConsole.WriteLine($"{attackTarget} is now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");
    }

    public abstract int CalculateDamage();
    public override string ToString() => Name.ToUpper();
}


// Defines the different damage types that an attack can have
public enum DamageType { Normal, Decoding }
