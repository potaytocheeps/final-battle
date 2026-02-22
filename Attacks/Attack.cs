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
    private float _criticalHitChance;

    public Attack(string name, AttackType attackType, DamageType damageType)
    {
        NumberOfUsesLeft = 0;
        Name = name;
        AttackType = attackType;
        HitChance = 1; // By default, an attack has a 100% chance of hitting its target
        DamageType = damageType;
        _givesStatusEffect = false;

        if (DamageType != DamageType.Physical)
        {
            _givesStatusEffect = true;
            _statusEffectChance = GetStatusEffectChance();
        }
        else
        {
            // Physical attacks have a 20% chance to be a critical hit, dealing twice the damage
            _criticalHitChance = 0.20f;
        }
    }

    public virtual void DealDamage(Character user, Character attackTarget)
    {
        // The damage that an attack deals can vary per turn. It should be calculated
        // each time the attack is used during a turn
        int damageAmount = CalculateDamage();

        if (DamageType == DamageType.Physical)
        {
            bool isCriticalHit = Random.Shared.NextSingle() < _criticalHitChance;

            if (isCriticalHit)
            {
                damageAmount *= 2;
                ColoredConsole.WriteLine($"{this} was a critical hit!", ConsoleColor.Yellow);
            }
        }

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
        ColoredConsole.WriteLine($"{this} dealt {TextColor.ColorText($"{damageAmount} {damageType}", DamageType)} damage to {attackTarget}.");

        if (_givesStatusEffect)
        {
            if (Random.Shared.NextSingle() < _statusEffectChance) ApplyStatusEffect(attackTarget);
        }

        attackTarget.TakeDamage(damageAmount);
    }

    private void ApplyStatusEffect(Character attackTarget)
    {
        string statusEffectName = "";
        StatusEffect? statusEffect;
        DamageType damageType = DamageType;

        // A damage type of Decoding will randomly inflict one of the available status
        // effects onto the target of the attack
        if (DamageType == DamageType.Decoding)
        {
            while (damageType == DamageType.Physical || damageType == DamageType.Decoding)
            {
                int random = Random.Shared.Next(Enum.GetNames<DamageType>().Count());
                damageType = (DamageType)random;
            }
        }

        statusEffect = damageType switch
        {
            DamageType.Poison   => new Poisoned(),
            DamageType.Electric => new Electrified(),
            DamageType.Fire     => new Burned(attackTarget.MaxHP),
            _                   => null
        };

        if (statusEffect != null)
        {
            attackTarget.ApplyStatusEffect(statusEffect);
            statusEffectName = statusEffect.StatusEffectName;
        }

        if (attackTarget is MylaraAndSkorin) ColoredConsole.Write($"{attackTarget} have been ");
        else ColoredConsole.Write($"{attackTarget} has been ");

        ColoredConsole.WriteLine($"{TextColor.ColorText(statusEffectName, damageType)}.");
    }

    public float GetStatusEffectChance()
    {
        return DamageType switch
        {
            DamageType.Electric => 0.33f, // 33% chance of applying its status effect onto the target
            DamageType.Poison   => 0.50f, // 50% chance
            DamageType.Fire     => 0.40f, // 40% chance
            DamageType.Decoding => 0.75f, // 75% chance
            _                   => 0,
        };
    }

    public abstract int CalculateDamage();
    public override string ToString() => Name.ToUpper();
}


// Defines the different damage types that an attack can have
public enum DamageType { Physical, Decoding, Poison, Electric, Fire }
