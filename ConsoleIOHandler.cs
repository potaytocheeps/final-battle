public class ConsoleIOHandler()
{
    public static void DisplayBattleInfo(Player player1, Player player2, int battleNumber)
    {
        string player1Party = string.Join(", ", player1.Party.Characters);
        string player2Party = string.Join(", ", player2.Party.Characters);
        string battleInfo = $"BATTLE {battleNumber}: {player1Party} vs. {player2Party}";
        int characterCount = 6;

        ColoredConsole.WriteLine(new string('=', battleInfo.Count() + (characterCount * 2)) + "\n");
        ColoredConsole.Write("".PadRight(characterCount));
        ColoredConsole.WriteLine(battleInfo);
        ColoredConsole.WriteLine("\n" + new string('=', battleInfo.Count() + (characterCount * 2)) + "\n");
    }

    public static void DisplayBattleStatus(Party player1Party, Party player2Party, Character currentCharacter, int currentRound)
    {
        // When the battle's number of rounds gets into the double digits, the formatting of the top row of
        // the battle status display gets misaligned due to the extra digit added. This fixes that visual misalignment
        if (currentRound < 10) ColoredConsole.WriteLine($"===================== ROUND {currentRound} =============================\n");
        else ColoredConsole.WriteLine($"===================== ROUND {currentRound} ============================\n");

        DisplayPartyInfo(player1Party, currentCharacter);
        ColoredConsole.WriteLine("\n------------------------ vs -------------------------------\n");
        DisplayPartyInfo(player2Party, currentCharacter, addPadding: true);
        ColoredConsole.WriteLine("\n===========================================================");
    }

    public static void DisplayPartyInfo(Party currentParty, Character currentCharacter, bool addPadding = false)
    {
        foreach (Character character in currentParty.Characters)
        {
            // Padding is added only to player 2's party, to keep it aligned to the right
            // side of the 'vs.' line in the battle status information
            if (addPadding) ColoredConsole.Write("".PadRight(30));

            // The character whose turn it is will be colored differently to distinguish it from the others.
            // This creates a visual difference to show that this is the currently selected character
            if (character == currentCharacter) ColoredConsole.Write($"{character}", ConsoleColor.Yellow);
            else ColoredConsole.Write($"{character}");

            ColoredConsole.Write($" (HP: {character.CurrentHP}/{character.MaxHP})");

            if (character.HasGearEquipped)
            {
                string gearEquipped = string.Join(", ", character.EquippedGear);

                ColoredConsole.Write($" ({gearEquipped})");
            }

            ColoredConsole.WriteLine("");
        }
    }

    public static void DisplaySelectionMenu(List<string> selectionOptions, bool isSubMenu = true)
    {
        for (int option = 0; option < selectionOptions.Count; option++)
        {
            ColoredConsole.WriteLine($"""
                {option + 1} - {selectionOptions[option]}
                """, ConsoleColor.Gray);
        }

        if (isSubMenu)
        {
            ColoredConsole.WriteLine
            (
                $"{selectionOptions.Count + 1} - Return to previous menu",
                ConsoleColor.Gray
            );
        }
    }

    public static int AskUserForSelection(int numberOfOptions, string prompt, bool isSubMenu = true)
    {
        if (isSubMenu) numberOfOptions += 1;

        while (true)
        {
            string choice = ColoredConsole.PromptUser(prompt, ConsoleColor.Gray).ToLower();

            if (int.TryParse(choice, out int selection))
            {
                selection -= 1;

                if (selection >= 0 && selection < numberOfOptions) return selection;
            }

            ColoredConsole.WriteLine($"Invalid input. Please select one of the available options (1-{numberOfOptions}).",
                                     ConsoleColor.DarkRed);
            continue;
        }
    }

    public static void WaitForPlayerConfirmation()
    {
        // Allow player to continue game when they are ready
        Console.Write("\nPress any key to continue");
        Console.ReadKey(intercept: true);

        // Deletes the previous line printed to the console and returns to the beginning of that line
        Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
    }
}
