/// <summary>
/// A character in the game that is controlled by a player or the computer.
/// </summary>
public abstract class Character
{
    public string Name { get; }
    public int MaxHP { get; }
    public int CurrentHP { get; private set; }
    protected Dictionary<AttackType, Attack> _attacks;
    public IReadOnlyDictionary<AttackType, Attack> Attacks => _attacks;
    public Gear? EquippedGear { get; private set; }
    public bool HasGearEquipped { get; private set; }

    public Character(string name, int maxHP, Gear? startingGear = null)
    {
        Name = name.ToUpper();
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = new ();

        if (startingGear != null) EquipGear(startingGear);
    }

    public bool PerformAction(Player currentPlayer, ActionType action, Player enemyPlayer)
    {
        switch (action)
        {
            case ActionType.Nothing:
                Console.WriteLine($"{Name} did {action.ToString().ToUpper()}");
                break;
            case ActionType.Attack:
                (AttackType attackType, Character attackTarget) = currentPlayer.PerformAttack(currentCharacter: this, enemyPlayer.Party);

                // Get reference to attack object of the chosen attack type.
                // This reference will be used to get the attack data for the attack
                Attack attack = _attacks[attackType];

                Console.WriteLine($"{Name} used {attack} on {attackTarget}.");

                // The damage that an attack deals can vary per turn. It should be calculated
                // each time the attack is used during a turn
                attack.CalculateDamage(attack);

                DealDamage(attack.Damage, attackTarget);

                // Display the results of having performed the attack
                Console.WriteLine($"{attack} dealt {attack.Damage} damage to {attackTarget}.");
                Console.WriteLine($"{attackTarget} is now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");

                if (attackTarget.CurrentHP == 0)
                {
                    ColoredConsole.WriteLine($"{attackTarget} has been defeated!", ConsoleColor.Red);
                    enemyPlayer.Party.RemoveFromParty(attackTarget);
                }

                break;
            case ActionType.UseItem:
                if (currentPlayer.Party.Items.Count <= 0) // There are no more items in party's inventory
                {
                    ColoredConsole.WriteLine("Inventory is empty.", ConsoleColor.DarkRed);
                    return false; // Action could not be completed. Ask player to select an action again
                }

                Item item = currentPlayer.SelectItem();
                UseItem(user: this, target: this, item);

                currentPlayer.Party.RemoveItemFromInventory(item);
                break;
            case ActionType.Equip:
                if (currentPlayer.Party.Gear.Count <= 0) // There is no gear in party's inventory
                {
                    ColoredConsole.WriteLine("No gear available.", ConsoleColor.DarkRed);
                    return false; // Action could not be completed. Ask player to select an action again
                }

                Gear gear = currentPlayer.SelectGear();
                EquipGear(currentPlayer, gear);
                ColoredConsole.WriteLine($"{Name} equipped {gear.Name} and gained Special Attack: {gear.AttackProvided}");
                break;
        }

        // Selected action was successfully performed
        return true;
    }

    protected void DealDamage(int damage, Character attackTarget)
    {
        if (attackTarget.CurrentHP - damage <= 0) attackTarget.CurrentHP = 0;
        else attackTarget.CurrentHP -= damage;
    }

    protected void UseItem(Character user, Character target, Item item)
    {
        switch (item)
        {
            case HealthPotion potion:
                Heal(target, potion.HealAmount);
                ColoredConsole.WriteLine($"{user} used {potion}.");
                ColoredConsole.WriteLine($"{potion} heals {target} for {potion.HealAmount} HP.");
                ColoredConsole.WriteLine($"{target} is now at {CurrentHP}/{MaxHP} HP.");
                break;
        }
    }

    protected void Heal(Character healTarget, int healAmount)
    {
        if (healTarget.CurrentHP + healAmount > healTarget.MaxHP) healTarget.CurrentHP = healTarget.MaxHP;
        else healTarget.CurrentHP += healAmount;
    }

    protected void EquipGear(Player currentPlayer, Gear gearToEquip)
    {
        if (EquippedGear != null) // Character already has gear equipped
        {
            // Put previously equipped gear back into party's inventory
            currentPlayer.Party.AddGearToInventory(EquippedGear);
        }

        EquipGear(gearToEquip);
        currentPlayer.Party.RemoveGearFromInventory(gearToEquip);
    }

    protected void EquipGear(Gear gearToEquip)
    {
        EquippedGear = gearToEquip;
        HasGearEquipped = true;
        _attacks[gearToEquip.AttackProvided.AttackType] = gearToEquip.AttackProvided;
    }

    public override string ToString() => Name;
}
