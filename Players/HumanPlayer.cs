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
        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions: ["Attack", "Use Item", "Equip Gear", "Do Nothing"]);

        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: 4, prompt: "What do you want to do? ");

        return Enum.Parse<ActionType>(selection.ToString());
    }

    protected override Attack SelectAttack(Character currentCharacter)
    {
        if (currentCharacter.Attacks.Count == 1) return currentCharacter.Attacks.First();

        string standardAttackName = currentCharacter.Attacks.First().Name;
        string specialAttackName = currentCharacter.Attacks.Last().Name;

        List<string> selectionOptions =
        [
            $"Standard Attack ({standardAttackName})",
            $"Special Attack ({specialAttackName})",
        ];

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: selectionOptions.Count, prompt: "Select your attack: ");

        return currentCharacter.Attacks[selection];
    }

    protected override Character SelectAttackTarget(Party enemyParty)
    {
        if (enemyParty.Characters.Count == 1) return enemyParty.Characters.First();

        List<string> selectionOptions = [];

        foreach (Character character in enemyParty.Characters)
        {
            selectionOptions.Add(character.Name);
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: enemyParty.Characters.Count, prompt: "Select the target: ");

        return enemyParty.Characters[selection];
    }

    public override Item SelectItem()
    {
        List<Item> uniqueItems = Party.GetListOfUniqueItemsInInventory();
        List<string> selectionOptions = [];

        foreach (Item item in uniqueItems)
        {
            selectionOptions.Add($"{item.Name} ({Party.GetItemTypeCount(item)})");
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: uniqueItems.Count, prompt: "Select item: ");

        return uniqueItems[selection];
    }

    public override Gear SelectGear()
    {
        List<Gear> uniqueGear = Party.GetListOfUniqueGearInInventory();
        List<string> selectionOptions = [];

        foreach (Gear gear in uniqueGear)
        {
            selectionOptions.Add($"{gear.Name} ({Party.GetGearTypeCount(gear)})");
        }

        ConsoleIOHandler.DisplaySelectionMenu(selectionOptions);
        int selection = ConsoleIOHandler.AskUserForSelection(numberOfOptions: uniqueGear.Count, prompt: "Select gear: ");

        return uniqueGear[selection];
    }
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Attack, UseItem, Equip, Nothing }


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard, Special }
