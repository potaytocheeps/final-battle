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
        List<string> selectionOptions = ["Attack", "Use Item", "Equip Gear", "Unequip Gear", "Do Nothing"];

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions, isSubMenu: false);

        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: selectionOptions.Count,
                                                             prompt: "What do you want to do? ",
                                                             isSubMenu: false);

        return Enum.Parse<ActionType>(selection.ToString());
    }

    protected override bool TrySelectAttack(Character currentCharacter, out Attack selectedAttack)
    {
        List<string> selectionOptions = [];

        foreach (Attack attack in currentCharacter.Attacks)
        {
            string selectionOption = "";

            if (attack.DamageType != DamageType.Normal)
            {
                selectionOption = $"{attack.DamageType.ToString().ToUpper()} ";
            }

            selectionOption += $"{attack}";

            if (attack.NumberOfUsesLeft >= 1)
            {
                selectionOption += $" (Uses left: {attack.NumberOfUsesLeft})";
            }

            selectionOptions.Add(selectionOption);
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

    protected override bool TrySelectTarget(Party party, out Character target)
    {
        if (party.Characters.Count == 1)
        {
            target = party.Characters.First();
            return true;
        }

        List<string> selectionOptions = [];

        foreach (Character character in party.Characters)
        {
            selectionOptions.Add(character.Name);
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: party.Characters.Count, prompt: "Select the target: ");

        if (selection >= party.Characters.Count)
        {
            target = party.Characters.First();
            return false;
        }

        target = party.Characters[selection];
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

    public override bool TrySelectGear(IReadOnlyList<Gear> gearOptions, out Gear selectedGear, bool isEquipMenu = true)
    {
        List<string> selectionOptions = [];

        foreach (Gear gear in gearOptions)
        {
            string selectionOption = $"{gear.Name}";

            if (gear.AttackProvided != null) // This gear is a weapon that provides a special attack
            {
                selectionOption += $" (Uses left: {gear.AttackProvided.NumberOfUsesLeft})";
            }

            if (isEquipMenu)
            {
                int gearTypeCount = Party.GetGearTypeCount(gear);
                if (gearTypeCount > 1) selectionOption += $" (x{gearTypeCount})";
            }

            selectionOptions.Add(selectionOption);
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: gearOptions.Count, prompt: "Select gear: ");

        if (selection >= gearOptions.Count)
        {
            selectedGear = gearOptions[0];
            return false;
        }

        selectedGear = gearOptions[selection];
        return true;
    }
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Attack, UseItem, Equip, Unequip, Nothing }


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard, Special }
