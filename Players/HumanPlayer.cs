/// <summary>
/// A human-controlled player.
/// The player will be given the chance to select different choices for their
/// party of characters during their turn in a battle.
/// </summary>
public class HumanPlayer : Player
{
    public HumanPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound)
    {
        ActionType action;

        while (true)
        {
            action = SelectAction(currentCharacter);
            if (TryPerformAction(action, currentCharacter, enemyPlayer)) break;
        }
    }

    protected override ActionType SelectAction(Character _)
    {
        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions: ["Attack", "Use Item", "Equip Gear", "Do Nothing"], isSubMenu: false);

        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: 4, prompt: "What do you want to do? ", isSubMenu: false);

        return Enum.Parse<ActionType>(selection.ToString());
    }

    protected override bool TrySelectAttack(Character currentCharacter, out Attack selectedAttack)
    {
        if (currentCharacter.Attacks.Count == 1)
        {
            selectedAttack = currentCharacter.Attacks.First();
            return true;
        }

        List<string> selectionOptions = [];

        foreach (Attack attack in currentCharacter.Attacks)
        {
            selectionOptions.Add($"{attack}");
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: selectionOptions.Count, prompt: "Select your attack: ");

        if (selection >= selectionOptions.Count)
        {
            selectedAttack = currentCharacter.Attacks[0];
            return false;
        }

        selectedAttack = currentCharacter.Attacks[selection];
        return true;
    }

    protected override bool TrySelectAttackTarget(Party enemyParty, out Character attackTarget)
    {
        if (enemyParty.Characters.Count == 1)
        {
            attackTarget = enemyParty.Characters.First();
            return true;
        }

        List<string> selectionOptions = [];

        foreach (Character character in enemyParty.Characters)
        {
            selectionOptions.Add(character.Name);
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: enemyParty.Characters.Count, prompt: "Select the target: ");

        if (selection >= enemyParty.Characters.Count)
        {
            attackTarget = enemyParty.Characters.First();
            return false;
        }

        attackTarget = enemyParty.Characters[selection];
        return true;
    }

    public override bool TrySelectItem(out Item selectedItem)
    {
        List<Item> uniqueItems = Party.GetListOfUniqueItemsInInventory();
        List<string> selectionOptions = [];

        foreach (Item item in uniqueItems)
        {
            selectionOptions.Add($"{item.Name} ({Party.GetItemTypeCount(item)})");
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: uniqueItems.Count, prompt: "Select item: ");

        if (selection >= uniqueItems.Count)
        {
            selectedItem = uniqueItems[0];
            return false;
        }

        selectedItem = uniqueItems[selection];
        return true;
    }

    public override bool TrySelectGear(out Gear selectedGear)
    {
        List<Gear> uniqueGear = Party.GetListOfUniqueGearInInventory();
        List<string> selectionOptions = [];

        foreach (Gear gear in uniqueGear)
        {
            selectionOptions.Add($"{gear.Name} ({Party.GetGearTypeCount(gear)})");
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: uniqueGear.Count, prompt: "Select gear: ");

        if (selection >= uniqueGear.Count)
        {
            selectedGear = uniqueGear[0];
            return false;
        }

        selectedGear = uniqueGear[selection];
        return true;
    }
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Attack, UseItem, Equip, Nothing }


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard, Special }
