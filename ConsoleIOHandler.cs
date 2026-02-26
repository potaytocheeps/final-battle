public static class ConsoleIOHandler
{
    public static void DisplayBattleInfo(Player player1, Player player2, int battleNumber, bool isFinalBattle)
    {
        string player1Party = string.Join(", ", player1.Party.Characters);
        string player2Party = string.Join(", ", player2.Party.Characters);
        string battleInfo = $"BATTLE {battleNumber}: {player1Party} vs. {player2Party}";

        if (isFinalBattle)
        {
            battleInfo = $"FINAL BATTLE: {player1Party} vs. {player2Party}";
        }

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

            DisplayCharacterHealth(character);
            if (character.StatusEffects.Count > 0) DisplayStatusEffects(character);

            if (character.HasGearEquipped)
            {
                string gearEquipped = string.Join(", ", character.EquippedGear);

                ColoredConsole.Write($" ({gearEquipped})");
            }

            ColoredConsole.WriteLine("");
        }
    }

    private static void DisplayCharacterHealth(Character character)
    {
        float currentHealthPercentage = (float)character.CurrentHP / character.MaxHP;

        ColoredConsole.Write($" (HP: {character.CurrentHP}/{character.MaxHP})");
        if (currentHealthPercentage <= 0.20f) ColoredConsole.Write($"[!]", ConsoleColor.Red);
        else if (currentHealthPercentage <= 0.40f) ColoredConsole.Write($"[!]", ConsoleColor.Yellow);
    }

    private static void DisplayStatusEffects(Character character)
    {
        List<string> statusEffects = [];

        foreach (StatusEffect statusEffect in character.StatusEffects.Values)
        {
            char firstLetter = statusEffect.StatusEffectName[0];

            statusEffects.Add(TextColor.ColorText(firstLetter.ToString(), statusEffect.DamageType));
        }

        string statusEffectsText = string.Join(", ", statusEffects);

        ColoredConsole.Write($" [{statusEffectsText}]");
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

    public static void DisplayTitle()
    {
        ColoredConsole.WriteLine("""
        ==========================================================
        ||                                                      ||
        ||                  The Final Battle                    ||
        ||                                                      ||
        ==========================================================
        """);
    }

    public static PlayerMode SelectPlayerMode()
    {
        ColoredConsole.WriteLine("This game can be played in the following ways:");
        List<string> selectionOptions = ["Human vs. Computer", "Human vs. Human", "Computer vs. Computer"];

        DisplaySelectionMenu(selectionOptions, isSubMenu: false);

        int selection = AskUserForSelection(selectionOptions.Count, "Select player mode: ", isSubMenu: false);

        return (PlayerMode)selection;
    }

    public static bool TrySelectDifficulty(out Difficulty gameDifficulty)
    {
        ColoredConsole.WriteLine("\nThere are three difficulties.");
        ColoredConsole.WriteLine("As you win a battle, the next one becomes more difficult.");
        ColoredConsole.WriteLine("The last battle has you facing against the final boss: The Uncoded One");
        List<string> selectionOptions = ["Easy (3 battles)", "Medium (5 battles)", "Hard (8 battles)"];

        DisplaySelectionMenu(selectionOptions, isSubMenu: true);

        int selection = AskUserForSelection(selectionOptions.Count, "Select difficulty: ", isSubMenu: true);

        if (selection == selectionOptions.Count)
        {
            gameDifficulty = Difficulty.Easy;
            return false;
        }

        gameDifficulty = (Difficulty)selection;
        return true;
    }

    public static bool TryPlayAgain()
    {
        WaitForPlayerConfirmation();
        ColoredConsole.WriteLine("");

        while (true)
        {
            string input = ColoredConsole.PromptUser("Would you like to play again? ").ToLower();

            if (input == "y" || input == "yes")
            {
                Console.Clear();
                return true;
            }
            else if (input == "n" || input == "no")
            {
                return false;
            }

            ColoredConsole.WriteLine("Invalid input. Please enter either 'y' or 'n'.", ConsoleColor.DarkRed);
        }
    }

    public static void DisplayGameInformation()
    {
        ColoredConsole.WriteLine("\n");
        ColoredConsole.WriteLine("""
        GAME INFORMATION

        [Attacks]

            >---------Bite: (DMG: 2)------ Heals for amount of damage dealt
            >--Bone Crunch: (DMG: 1-3)
            >-Cannon Blast: (DMG: 1,3,10)- 3 damage every 3rd or 5th turn, 10 every 15th, 1 otherwise
            >--Cannon Shot: (DMG: 1-2)
            >------Grapple: (DMG: 2)------ 33% chance to steal a random piece of gear from target
            >--------Punch: (DMG: 1)
            >---Quick Shot: (DMG: 4)------ 50% chance to hit target
            >--------Slash: (DMG: 3)
            >---------Stab: (DMG: 2)
            >---Unraveling: (DMG: 4-6)

        [Gear]

            >--------Binary Helm: Gives Damage Buff and Stone Armor modifiers
            >-Cannon of Consolas: Gives special attack: Cannon Blast
            >-------------Dagger: Gives special attack: Stab
            >--------------Sword: Gives special attack: Slash
            >----------Vin's Bow: Gives special attack: Quick Shot

        [Items]

            (Health Potions)
                >------------Small: Heals 10 HP
                >-----------Medium: Heals 20 HP
                >------------Large: Heals 30 HP
                >----Simula's Soup: Fully restores HP

            (Cure Potions)
                >--------Burn Cure: Cures Burned status effect
                >-Electrified Cure: Cures Electrified status effect
                >------Poison Cure: Cures Poisoned status effect
                >---------Cure All: Cures all status effects

        [Modifiers]

            >--Damage Buff: +1 damage dealt
            >--Stone Armor: -1 damage taken
            >-Object Sight: -2 damage taken from Decoding damage type

        [Damage Types]

            >-Physical (20% chance to deal double damage)
            >-----Fire (33% chance to inflict Burn)
            >-Electric (33% chance to inflict Electrification)
            >---Poison (33% chance to inflict Poison)
            >-Decoding (75% chance to inflict a random status effect | 10% chance to deal double damage)

        [Status Effects]

            (Burn)
                >---Effect: Deals 5% of inflicted character's max HP as damage per turn
                >-Duration: 3 turns
            (Poison)
                >---Effect: Starting from the damage the attack dealt, deals 1 less damage per turn to inflicted character
                >-Duration: Until damage reaches 0
            (Electrification)
                >---Effect: Prevents inflicted character from acting during their turn
                >-Duration: 1 turn
        """);

        WaitForPlayerConfirmation();
    }
}
