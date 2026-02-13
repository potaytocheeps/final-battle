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

        if (startingGear != null) EquipGear(startingGear);
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

    public void EquipGear(Gear gearToEquip)
    {
        _equippedGear.Add(gearToEquip);
        _attacks.Add(gearToEquip.AttackProvided);
    }

    public override string ToString() => Name.ToUpper();
}
