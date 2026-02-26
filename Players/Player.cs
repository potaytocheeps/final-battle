/// <summary>
/// A Player in the game that controls a Party of Characters.
/// </summary>
public abstract class Player
{
    public Party Party { get; private set; }
    public int PlayerNumber { get; }

    public Player(Party party, int playerNumber)
    {
        Party = party;
        PlayerNumber = playerNumber;
    }

    public void SetParty(Party party) => Party = party;

    protected bool TryPerformAction(ActionType action, Character currentCharacter, Player enemyPlayer)
    {
        switch (action)
        {
            case ActionType.Nothing:
                ColoredConsole.WriteLine($"{currentCharacter} did {action.ToString().ToUpper()}");
                break;
            case ActionType.Attack:
                return TryPerformAttack(currentCharacter, enemyPlayer.Party);
            case ActionType.UseItem:
                return TryPerformItemUse(currentCharacter, Party);
            case ActionType.Equip:
                return TryPerformGearEquip(currentCharacter);
            case ActionType.Unequip:
                return TryPerformGearUnequip(currentCharacter);
            default:
                return false;
        }

        return true; // Action was successfully performed
    }

    private bool TryPerformAttack(Character currentCharacter, Party enemyParty)
    {
        Attack attack;
        Character attackTarget;

        while (true)
        {
            bool attackWasSelected = TrySelectAttack(currentCharacter, out attack);

            if (!attackWasSelected) return false;

            bool attackTargetWasSelected = TrySelectTarget(enemyParty, out attackTarget);

            if (attackTargetWasSelected) break;
        }

        currentCharacter.Attack(attack, attackTarget);

        if (attackTarget.CurrentHP == 0)
        {
            LootEnemyCharacter(attackTarget);
            enemyParty.RemoveFromParty(attackTarget);
        }

        return true;
    }

    private bool TryPerformItemUse(Character currentCharacter, Party currentParty)
    {
        if (Party.ItemInventory.Count <= 0) // There are no more items in party's inventory
        {
            ColoredConsole.WriteLine("Inventory is empty.", ConsoleColor.DarkRed);
            return false; // Action could not be completed. Ask player to select an action again
        }

        Item item;
        Character target;

        while (true)
        {
            bool itemWasSelected = TrySelectItem(out item);

            if (!itemWasSelected) return false;

            bool targetWasSelected = TrySelectItemTarget(currentCharacter, currentParty, item, out target);

            if (targetWasSelected) break;
        }

        currentCharacter.UseItem(target, item);
        Party.RemoveItemFromInventory(item);

        return true;
    }

    private bool TryPerformGearEquip(Character currentCharacter)
    {
        if (Party.GearInventory.Count <= 0) // There is no gear in party's inventory
        {
            ColoredConsole.WriteLine("No gear available.", ConsoleColor.DarkRed);
            return false; // Action could not be completed. Ask player to select an action again
        }

        bool gearWasSelected = TrySelectGear(Party.GetListOfUniqueGearInInventory(), out Gear gear);

        if (!gearWasSelected) return false;

        currentCharacter.EquipGear(gear);
        Party.RemoveGearFromInventory(gear);

        return true;
    }

    private bool TryPerformGearUnequip(Character currentCharacter)
    {
        if (!currentCharacter.HasGearEquipped)
        {
            ColoredConsole.WriteLine("No gear equipped.", ConsoleColor.DarkRed);
            return false; // Action could not be completed. Ask player to select an action again
        }

        bool gearWasSelected = TrySelectGear(currentCharacter.EquippedGear, out Gear gear, isEquipMenu: false);

        if (!gearWasSelected) return false;

        currentCharacter.UnequipGear(gear);
        Party.AddGearToInventory(gear);
        ColoredConsole.WriteLine($"{gear} was added back to inventory.");

        return true;
    }

    public void LootEnemyCharacter(Character character)
    {
        // Loot the defeated character's equipped gear
        if (character.EquippedGear != null)
        {
            foreach (Gear gear in character.EquippedGear)
            {
                Party.AddGearToInventory(gear);
                ColoredConsole.WriteLine($"\n{character} dropped {gear}!");
                ColoredConsole.WriteLine($"{gear} was added to Player {PlayerNumber} party's inventory.");
            }
        }
    }

    public void Loot(Party losingParty)
    {
        bool partyWasLooted = false;

        if (losingParty.ItemInventory.Count > 0)
        {
            LootItems(losingParty);
            partyWasLooted = true;
        }

        if (losingParty.GearInventory.Count > 0)
        {
            LootGear(losingParty);
            partyWasLooted = true;
        }

        if (partyWasLooted) ConsoleIOHandler.WaitForPlayerConfirmation();
    }

    private void LootItems(Party losingParty)
    {
        // Loot enemy party's items
        foreach (Item item in losingParty.ItemInventory)
        {
            Party.AddItemToInventory(item);
        }

        List<Item> uniqueItems = losingParty.GetListOfUniqueItemsInInventory();

        ColoredConsole.WriteLine($"Player {PlayerNumber} looted the following items from the enemy's party:");

        // Display items looted
        foreach (Item item in uniqueItems)
        {
            string itemLooted = $"{item}";

            int itemTypeCount = losingParty.GetItemTypeCount(item);
            if (itemTypeCount > 1) itemLooted += $" (x{itemTypeCount})";

            ColoredConsole.WriteLine($"- {itemLooted}");
        }
    }

    private void LootGear(Party losingParty)
    {
        // Loot enemy party's gear
        foreach (Gear gear in losingParty.GearInventory)
        {
            Party.AddGearToInventory(gear);
        }

        List<Gear> uniqueGear = losingParty.GetListOfUniqueGearInInventory();

        ColoredConsole.WriteLine($"Player {PlayerNumber} looted the following gear from the enemy's party:");

        // Display gear looted
        foreach (Gear gear in uniqueGear)
        {
            string gearLooted = $"{gear}";

            if (gear.AttackProvided != null) // This gear is a weapon that provides a special attack
            {
                gearLooted += $" (Uses left: {gear.AttackProvided.NumberOfUsesLeft})";
            }

            int gearTypeCount = losingParty.GetGearTypeCount(gear);
            if (gearTypeCount > 1) gearLooted += $" (x{gearTypeCount})";

            ColoredConsole.WriteLine($"- {gearLooted}");
        }
    }

    protected virtual bool TrySelectItemTarget(Character currentCharacter, Party party, Item item, out Character target)
    {
        return TrySelectTarget(party, out target);
    }

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound);
    protected abstract ActionType SelectAction(Character currentCharacter);
    protected abstract bool TrySelectAttack(Character currentCharacter, out Attack attack);
    protected abstract bool TrySelectTarget(Party party, out Character target);
    public abstract bool TrySelectItem(out Item item);
    public abstract bool TrySelectGear(IReadOnlyList<Gear> gearOptions, out Gear gear, bool isEquipMenu = true);
}
