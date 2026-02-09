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
                PerformAttack(currentCharacter, enemyPlayer.Party);
                break;
            case ActionType.UseItem:
                return TryPerformItemUse(currentCharacter);
            case ActionType.Equip:
                return TryPerformGearEquip(currentCharacter);
        }

        return true; // Action was successfully performed
    }

    private void PerformAttack(Character currentCharacter, Party enemyParty)
    {
        Attack attack = SelectAttack(currentCharacter);
        Character attackTarget = SelectAttackTarget(enemyParty);

        currentCharacter.Attack(attack, attackTarget);

        if (attackTarget.CurrentHP == 0)
        {
            ColoredConsole.WriteLine($"{attackTarget} has been defeated!", ConsoleColor.Red);
            enemyParty.RemoveFromParty(attackTarget);
        }
    }

    private bool TryPerformItemUse(Character currentCharacter)
    {
        if (Party.ItemInventory.Count <= 0) // There are no more items in party's inventory
        {
            ColoredConsole.WriteLine("Inventory is empty.", ConsoleColor.DarkRed);
            return false; // Action could not be completed. Ask player to select an action again
        }

        Item? item = SelectItem();

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

        Gear? gear = SelectGear();
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

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound);
    protected abstract ActionType SelectAction(Character currentCharacter);
    protected abstract Attack SelectAttack(Character currentCharacter);
    protected abstract Character SelectAttackTarget(Party enemyParty);
    public abstract Item? SelectItem();
    public abstract Gear? SelectGear();

}
