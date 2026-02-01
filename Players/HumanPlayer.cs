/// <summary>
/// A human-controlled player.
/// The player will be given the chance to select different choices for their
/// party of characters during their turn in a battle.
/// </summary>
public class HumanPlayer : Player
{
    public HumanPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer)
    {
        ActionType action = SelectAction();
        currentCharacter.PerformAction(currentPlayer: this, action, enemyPlayer);
    }

    protected override ActionType SelectAction()
    {
        Console.WriteLine($"""
            1 - Attack
            2 - Do Nothing
            """);

        while (true)
        {
            Console.Write("What do you want to do? ");

            string? choice = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(choice)) continue;

            (ActionType actionType, bool success) = choice switch
            {
                "1" or "attack"                  => (ActionType.Attack, true),
                "2" or "do nothing" or "nothing" => (ActionType.Nothing, true),
                _                                => (ActionType.Nothing, false)
            };

            if (success) return actionType;
            else Console.WriteLine("Invalid input. Please select one of the available options.");
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

        Console.WriteLine($"""
            1 - Standard Attack ({standardAttackName})
            """);

        while (true)
        {
            Console.Write("Select your attack: ");

            string? choice = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(choice)) continue;

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

            Console.WriteLine("Invalid input. Please select one of the available options.");
            continue;
        }
    }

    protected override Character SelectAttackTarget(Party enemyParty)
    {
        if (enemyParty.Characters.Count == 1) return enemyParty.Characters.First();

        int characterNumber = 1;
        foreach (Character character in enemyParty.Characters)
        {
            Console.WriteLine($"""
                {characterNumber} - {character.Name}
                """);

            characterNumber++;
        }

        while (true)
        {
            Console.Write("Select the target: ");

            string? choice = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(choice)) continue;

            for (int index = 0; index < enemyParty.Characters.Count; index++)
            {
                Character character = enemyParty.Characters[index];

                if (choice == (index + 1).ToString() || choice == character.Name.ToLower()) return character;
            }

            Console.WriteLine("Invalid input. Please select one of the available options.");
            continue;
        }
    }
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Nothing, Attack }


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard }
