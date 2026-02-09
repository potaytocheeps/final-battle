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
    public bool HasGearEquipped { get; private set; }

    public Character(string name, int maxHP, Gear? startingGear = null)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = [];

        if (startingGear != null) EquipGear(startingGear);
    }

    public void Attack(Attack attack, Character attackTarget)
    {
        // The damage that an attack deals can vary per turn. It should be calculated
        // each time the attack is used during a turn
        attack.CalculateDamage();

        attackTarget.TakeDamage(attack.Damage);

        // Display the results of having performed the attack
        ColoredConsole.WriteLine($"{this} used {attack} on {attackTarget}.");
        ColoredConsole.WriteLine($"{attack} dealt {attack.Damage} damage to {attackTarget}.");
        ColoredConsole.WriteLine($"{attackTarget} is now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");
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

    public Gear? EquipGear(Gear? gearToEquip)
    {
        Gear? previouslyEquippedGear = null;

        // Character already has gear equipped
        if (EquippedGear != null) previouslyEquippedGear = EquippedGear;

        EquippedGear = gearToEquip;
        HasGearEquipped = true;

        if (gearToEquip != null)
        {
            _attacks.Add(gearToEquip.AttackProvided);
        }

        return previouslyEquippedGear;
    }

    public override string ToString() => Name.ToUpper();
}
