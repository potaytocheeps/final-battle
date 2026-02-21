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
    public DamageType DamageType { get; }
    public int NumberOfUsesLeft { get; protected set; }
    private bool _givesStatusEffect;
    private float _statusEffectChance;

    public Attack(string name, AttackType attackType, DamageType damageType)
    {
        NumberOfUsesLeft = 0;
        Name = name;
        AttackType = attackType;
        HitChance = 1; // By default, an attack has a 100% chance of hitting its target
        DamageType = damageType;
        _givesStatusEffect = false;

        if (DamageType != DamageType.Normal && DamageType != DamageType.Decoding)
        {
            _givesStatusEffect = true;
            _statusEffectChance = GetStatusEffectChance();
        }
    }

    public virtual void DealDamage(Character user, Character attackTarget)
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
                damageAmount = modifier.CalculateModifiedDamage(user, damageAmount, DamageType);
            }
        }

        if (attackTarget.HasDefensiveDamageModifier)
        {
            List<Modifier> defensiveModifiers = attackTarget.Modifiers[ModifierType.Defensive];

            foreach (Modifier modifier in defensiveModifiers)
            {
                damageAmount = modifier.CalculateModifiedDamage(attackTarget, damageAmount, DamageType);
            }
        }

        string damageType = DamageType.ToString().ToUpper();

        // Display results of having performed the attack
        ColoredConsole.WriteLine($"{this} dealt {damageAmount} {damageType} damage to {attackTarget}.");

        if (_givesStatusEffect)
        {
            if (Random.Shared.NextSingle() < _statusEffectChance) ApplyStatusEffect(attackTarget);
        }

        attackTarget.TakeDamage(damageAmount);
    }

    private void ApplyStatusEffect(Character attackTarget)
    {
        string statusEffectName = "";

        StatusEffect? statusEffect = DamageType switch
        {
            DamageType.Poison   => new Poisoned(),
            DamageType.Electric => new Electrified(),
            _                   => null
        };

        if (statusEffect != null)
        {
            attackTarget.ApplyStatusEffect(statusEffect);
            statusEffectName = statusEffect.StatusEffectName;
        }

        if (attackTarget is MylaraAndSkorin) ColoredConsole.WriteLine($"{attackTarget} have been {statusEffectName}.");
        else ColoredConsole.WriteLine($"{attackTarget} has been {statusEffectName}.");
    }

    public float GetStatusEffectChance()
    {
        return DamageType switch
        {
            DamageType.Electric => 0.33f, // 33% chance of applying its status effect onto the target
            DamageType.Poison   => 0.50f, // 50% chance
            _                   => 0,
        };
    }

    public abstract int CalculateDamage();
    public override string ToString() => Name.ToUpper();
}


// Defines the different damage types that an attack can have
public enum DamageType { Normal, Decoding, Poison, Electric }
