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
    private Dictionary<StatusEffectType, StatusEffect> _statusEffects;
    public IReadOnlyDictionary<StatusEffectType, StatusEffect> StatusEffects => _statusEffects;

    public Character(string name, int maxHP, Gear? startingGear = null)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = [];
        _modifiers = [];
        _equippedGear = [];
        _statusEffects = [];

        if (startingGear != null) EquipGear(startingGear, isStartingGear: true);
    }

    public Character(string name, Gear? startingGear, Modifier? startingModifier, int maxHP)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = [];
        _modifiers = [];
        _equippedGear = [];
        _statusEffects = [];

        if (startingGear != null)
        {
            EquipGear(startingGear, isStartingGear: true);
        }

        if (startingModifier != null)
        {
            if (_modifiers.ContainsKey(startingModifier.ModifierType))
            {
                _modifiers[startingModifier.ModifierType].Add(startingModifier);
            }
            else
            {
                _modifiers.Add(startingModifier.ModifierType, [startingModifier]);
            }
        }
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

        if (this is MylaraAndSkorin) ColoredConsole.WriteLine($"{this} are now at {CurrentHP}/{MaxHP} HP.");
        else ColoredConsole.WriteLine($"{this} is now at {CurrentHP}/{MaxHP} HP.");

        if (CurrentHP == 0)
        {
            if (this is MylaraAndSkorin) ColoredConsole.WriteLine($"{this} have been defeated!", ConsoleColor.Red);
            else ColoredConsole.WriteLine($"{this} has been defeated!", ConsoleColor.Red);
        }
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

    public void UnequipGear(Gear gearToRemove, bool gearWasStolen = false)
    {
        _equippedGear.Remove(gearToRemove);

        if (gearWasStolen) ColoredConsole.WriteLine($"{this} lost {gearToRemove} and also lost:");
        else ColoredConsole.WriteLine($"{this} unequipped {gearToRemove} and lost:");

        if (gearToRemove.AttackProvided != null)
        {
            _attacks.Remove(gearToRemove.AttackProvided);
            ColoredConsole.WriteLine($"- Special attack: {gearToRemove.AttackProvided}");
        }

        if (gearToRemove.ProvidesModifiers)
        {
            foreach (Modifier modifier in gearToRemove.ModifiersProvided)
            {
                if (_modifiers.ContainsKey(modifier.ModifierType))
                {
                    // If this character only had one modifier of this type, remove the whole
                    // entry from the dictionary, otherwise only remove the modifier from the list
                    if (_modifiers[modifier.ModifierType].Count <= 1) _modifiers.Remove(modifier.ModifierType);
                    else _modifiers[modifier.ModifierType].Remove(modifier);

                    ColoredConsole.WriteLine($"- {modifier.ModifierType} attack modifier: {modifier}");
                }
            }
        }
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (!_statusEffects.ContainsKey(statusEffect.StatusEffectType))
        {
            _statusEffects.Add(statusEffect.StatusEffectType, statusEffect);
        }
        else
        {
            _statusEffects[statusEffect.StatusEffectType] = statusEffect;
        }
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        _statusEffects.Remove(statusEffect.StatusEffectType);
    }

    public void RemoveAllStatusEffects()
    {
        _statusEffects = [];
    }

    public override string ToString() => Name.ToUpper();
}
