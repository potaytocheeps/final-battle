/// <summary>
/// A character in the game that is controlled by a player or the computer.
/// </summary>
public abstract class Character
{
    public string Name { get; }
    public int MaxHP { get; }
    public int CurrentHP { get; private set; }
    protected List<Attack> _attacks;
    public IReadOnlyList<Attack> Attacks => _attacks;
    public Gear? EquippedGear { get; private set; }
    public bool HasGearEquipped => EquippedGear != null;
    protected Dictionary<ModifierType, List<Modifier>> _modifiers;
    private bool HasDamageModifier => _modifiers.Count > 0;

    public Character(string name, int maxHP, Gear? startingGear = null)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = [];
        _modifiers = [];

        if (startingGear != null) EquipGear(startingGear);
    }

    public void Attack(Attack attack, Character attackTarget)
    {
        // The damage that an attack deals can vary per turn. It should be calculated
        // each time the attack is used during a turn
        int damageDealt = attack.CalculateDamage();

        // Determine whether attack will land, based on its hit chance
        bool attackLanded = Random.Shared.NextSingle() < attack.HitChance;

        // Display the results of having performed the attack
        ColoredConsole.WriteLine($"{this} used {attack} on {attackTarget}.");

        if (attackLanded)
        {
            // Modify damage dealt if attack target has any damage modifiers
            if (attackTarget.HasDamageModifier)
            {
                bool hasDefensiveModifier = attackTarget._modifiers.ContainsKey(ModifierType.Defensive);

                if (hasDefensiveModifier)
                {
                    List<Modifier> defensiveModifiers = attackTarget._modifiers[ModifierType.Defensive];

                    foreach (Modifier modifier in defensiveModifiers)
                    {
                        damageDealt = modifier.CalculateModifiedDamage(damageDealt);
                    }
                }
            }

            attackTarget.TakeDamage(damageDealt);
            ColoredConsole.WriteLine($"{attack} dealt {damageDealt} damage to {attackTarget}.");
            ColoredConsole.WriteLine($"{attackTarget} is now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");
        }
        else ColoredConsole.WriteLine($"{attack} missed!");
    }

    public void TakeDamage(int damageAmount)
    {
        if (CurrentHP - damageAmount <= 0) CurrentHP = 0;
        else CurrentHP -= damageAmount;
    }

    public void UseItem(Character target, Item item)
    {
        switch (item)
        {
            case HealthPotion potion:
                potion.Heal(user: this, healTarget: target);
                break;
        }
    }

    public void Heal(int healAmount)
    {
        if (CurrentHP + healAmount > MaxHP) CurrentHP = MaxHP;
        else CurrentHP += healAmount;
    }

    public Gear? EquipGear(Gear gearToEquip)
    {
        Gear? previouslyEquippedGear = null;

        // Character already has gear equipped
        if (EquippedGear != null)
        {
            previouslyEquippedGear = EquippedGear;
            _attacks.Remove(EquippedGear.AttackProvided);
        }

        EquippedGear = gearToEquip;
        _attacks.Add(gearToEquip.AttackProvided);

        return previouslyEquippedGear;
    }

    public override string ToString() => Name.ToUpper();
}
