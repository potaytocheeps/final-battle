/// <summary>
/// A Player in the game that controls a Party of Characters.
/// </summary>
public abstract class Player
{
    public Party Party { get; private set; }
    public int PlayerNumber { get; }
    private static int _instanceCount = 0; // Used to keep track of who is player 1 and who is player 2

    public Player(Party party)
    {
        Party = party;
        _instanceCount++;
        PlayerNumber = _instanceCount;
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
                return TryPerformItemUse(currentCharacter);
            case ActionType.Equip:
                return TryPerformGearEquip(currentCharacter);
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

            bool attackTargetWasSelected = TrySelectAttackTarget(enemyParty, out attackTarget);

            if (attackTargetWasSelected) break;
        }

        currentCharacter.Attack(attack, attackTarget);

        if (attackTarget.CurrentHP == 0)
        {
            ColoredConsole.WriteLine($"{attackTarget} has been defeated!", ConsoleColor.Red);

            // Loot the defeated character's equipped gear
            if (attackTarget.EquippedGear != null)
            {
                Gear gear = attackTarget.EquippedGear;

                Party.AddGearToInventory(gear);
                ColoredConsole.WriteLine($"\n{attackTarget} dropped {gear}!");
                ColoredConsole.WriteLine($"{gear} was added to Player {PlayerNumber} party's inventory.");
            }

            enemyParty.RemoveFromParty(attackTarget);
        }

        return true;
    }

    private bool TryPerformItemUse(Character currentCharacter)
    {
        if (Party.ItemInventory.Count <= 0) // There are no more items in party's inventory
        {
            ColoredConsole.WriteLine("Inventory is empty.", ConsoleColor.DarkRed);
            return false; // Action could not be completed. Ask player to select an action again
        }

        bool itemWasSelected = TrySelectItem(out Item? item);

        if (!itemWasSelected) return false;

        if (item != null)
        {
            currentCharacter.UseItem(target: currentCharacter, item);
            Party.RemoveItemFromInventory(item);
        }

        return true;
    }

    private bool TryPerformGearEquip(Character currentCharacter)
    {
        if (Party.GearInventory.Count <= 0) // There is no gear in party's inventory
        {
            ColoredConsole.WriteLine("No gear available.", ConsoleColor.DarkRed);
            return false; // Action could not be completed. Ask player to select an action again
        }

        bool gearWasSelected = TrySelectGear(out Gear? gear);

        if (!gearWasSelected) return false;

        Gear? previouslyEquippedGear = null;

        if (gear != null)
        {
            previouslyEquippedGear = currentCharacter.EquipGear(gear);
            ColoredConsole.WriteLine($"{currentCharacter} equipped {gear} and gained Special Attack: {gear.AttackProvided}");
            Party.RemoveGearFromInventory(gear);
        }

        if (previouslyEquippedGear != null) Party.AddGearToInventory(previouslyEquippedGear);

        return true;
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
            ColoredConsole.WriteLine($"- {item} ({losingParty.GetItemTypeCount(item)})");
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
            ColoredConsole.WriteLine($"- {gear} ({losingParty.GetGearTypeCount(gear)})");
        }
    }

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound);
    protected abstract ActionType SelectAction(Character currentCharacter);
    protected abstract bool TrySelectAttack(Character currentCharacter, out Attack attack);
    protected abstract bool TrySelectAttackTarget(Party enemyParty, out Character attackTarget);
    public abstract bool TrySelectItem(out Item? item);
    public abstract bool TrySelectGear(out Gear? gear);
}
