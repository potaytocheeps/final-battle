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
    private List<Gear> _equippedGear;
    public IReadOnlyList<Gear> EquippedGear => _equippedGear;
    public bool HasGearEquipped => EquippedGear.Count > 0;
    protected Dictionary<ModifierType, List<Modifier>> _modifiers;
    public Dictionary<ModifierType, List<Modifier>> Modifiers => _modifiers;
    public bool HasDefensiveDamageModifier => _modifiers.ContainsKey(ModifierType.Defensive);
    public bool HasOffensiveDamageModifier => _modifiers.ContainsKey(ModifierType.Offensive);

    public Character(string name, int maxHP, Gear? startingGear = null)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = [];
        _modifiers = [];
        _equippedGear = [];

        if (startingGear != null) EquipGear(startingGear, isStartingGear: true);
    }

    public void Attack(Attack attack, Character attackTarget)
    {
        // Determine whether attack will land, based on its hit chance
        bool attackLanded = Random.Shared.NextSingle() < attack.HitChance;

        // Display the results of having performed the attack
        ColoredConsole.WriteLine($"{this} used {attack} on {attackTarget}.");

        if (attackLanded) attack.DealDamage(user: this, attackTarget);
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

    public void EquipGear(Gear gearToEquip, bool isStartingGear = false)
    {
        _equippedGear.Add(gearToEquip);

        if (gearToEquip.AttackProvided != null) _attacks.Add(gearToEquip.AttackProvided);

        if (!isStartingGear)
        {
            ColoredConsole.WriteLine($"{this} equipped {gearToEquip} and gained:");
            if (gearToEquip.AttackProvided != null) ColoredConsole.WriteLine($"- Special attack: {gearToEquip.AttackProvided}");
        }

        if (gearToEquip.ProvidesModifiers)
        {
            foreach (Modifier modifier in gearToEquip.ModifiersProvided)
            {
                // If this character didn't already have this type of modifier, create a new dictionary
                // entry and new list with this modifier as its only element
                if (!_modifiers.ContainsKey(modifier.ModifierType))
                {
                    _modifiers.Add(modifier.ModifierType, [modifier]);
                }
                else // Add the modifier to the list that already exists
                {
                    _modifiers[modifier.ModifierType].Add(modifier);
                }

                if (!isStartingGear) ColoredConsole.WriteLine($"- {modifier.ModifierType} attack modifier: {modifier}");
            }
        }
    }

    public override string ToString() => Name.ToUpper();
}
