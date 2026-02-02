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
        if (_playerNumber == 1) Battle.DisplayBattleStatus(player1Party: Party, player2Party: enemyPlayer.Party, currentCharacter, currentRound);
        else Battle.DisplayBattleStatus(player1Party: enemyPlayer.Party, player2Party: Party, currentCharacter, currentRound);

        ColoredConsole.WriteLine($"Player {_playerNumber}");
        ColoredConsole.WriteLine($"It is {currentCharacter}'s turn...");

        while (true)
        {
            ActionType action = SelectAction(currentCharacter);
            bool actionWasSuccessful = currentCharacter.PerformAction(currentPlayer: this, action, enemyPlayer);

            if (actionWasSuccessful) break;
        }
    }

    protected override ActionType SelectAction(Character _)
    {
        ColoredConsole.WriteLine($"""
            1 - Attack
            2 - Use Item
            3 - Do Nothing
            """, ConsoleColor.Gray);

        while (true)
        {
            string choice = ColoredConsole.PromptUser("What do you want to do? ", ConsoleColor.Gray).ToLower();
            choice = choice.ToLower();

            (ActionType actionType, bool success) = choice switch
            {
                "1" or "attack"                      => (ActionType.Attack, true),
                "2" or "use item" or "use" or "item" => (ActionType.UseItem, true),
                "3" or "do nothing" or "nothing"     => (ActionType.Nothing, true),
                _                                    => (ActionType.Nothing, false)
            };

            if (success) return actionType;
            else ColoredConsole.WriteLine("Invalid input. Please select one of the available options.", ConsoleColor.DarkRed);
        }
    }

    public override (AttackType, Character) PerformAttack(Character currentCharacter, Party enemyParty)
    {
        AttackType attackType = SelectAttack(currentCharacter);
        Character attackTarget = SelectAttackTarget(enemyParty);

        return (attackType, attackTarget);
    }

    protected override AttackType SelectAttack(Character currentCharacter)
    {
        if (currentCharacter.Attacks.Count == 1) return currentCharacter.Attacks.First().Key;

        string standardAttackName = currentCharacter.Attacks[AttackType.Standard].Name;

        ColoredConsole.WriteLine($"""
            1 - Standard Attack ({standardAttackName})
            """, ConsoleColor.Gray);

        while (true)
        {
            string choice = ColoredConsole.PromptUser("Select your attack: ", ConsoleColor.Gray).ToLower();

            for (int index = 0; index < currentCharacter.Attacks.Count; index++)
            {
                string attackName = currentCharacter.Attacks.ElementAt(index).Value.Name.ToLower();

                if (choice == (index + 1).ToString() ||
                    choice == "standard" ||
                    choice == "standard attack" ||
                    choice == attackName)
                {
                    return currentCharacter.Attacks.ElementAt(index).Key;
                }
            }

            ColoredConsole.WriteLine("Invalid input. Please select one of the available options.", ConsoleColor.DarkRed);
            continue;
        }
    }

    protected override Character SelectAttackTarget(Party enemyParty)
    {
        if (enemyParty.Characters.Count == 1) return enemyParty.Characters.First();

        int characterNumber = 1;
        foreach (Character character in enemyParty.Characters)
        {
            ColoredConsole.WriteLine($"""
                {characterNumber} - {character.Name}
                """, ConsoleColor.Gray);

            characterNumber++;
        }

        while (true)
        {
            string choice = ColoredConsole.PromptUser("Select the target: ", ConsoleColor.Gray).ToLower();

            for (int index = 0; index < enemyParty.Characters.Count; index++)
            {
                Character character = enemyParty.Characters[index];

                if (choice == (index + 1).ToString() || choice == character.Name.ToLower()) return character;
            }

            ColoredConsole.WriteLine("Invalid input. Please select one of the available options.", ConsoleColor.DarkRed);
            continue;
        }
    }

    public override Item SelectItem()
    {
        List<Item> uniqueItems = Party.GetListOfUniqueItemsInInventory();

        int itemNumber = 1;
        foreach (Item item in uniqueItems)
        {
            // Capitalize only the first letter of the item's name
            string itemName = item.Name[0] + item.Name.Substring(1).ToLower();

            ColoredConsole.WriteLine($"""
                {itemNumber} - {itemName} ({Party.GetItemTypeCount<Item>()})
                """, ConsoleColor.Gray);

            itemNumber++;
        }

        while (true)
        {
            string choice = ColoredConsole.PromptUser("Select item: ", ConsoleColor.Gray).ToLower();

            for (int index = 0; index < uniqueItems.Count; index++)
            {
                Item item = uniqueItems[index];

                if (choice == (index + 1).ToString() || choice == item.Name.ToLower()) return item;
            }

            ColoredConsole.WriteLine("Invalid input. Please select one of the available options.", ConsoleColor.DarkRed);
            continue;
        }
    }
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Nothing, Attack, UseItem }


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard }
