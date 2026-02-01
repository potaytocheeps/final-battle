GameplayMode gameplayMode = SelectGameplayMode();
Console.WriteLine();
string playerName = ColoredConsole.PromptUser("Enter the name for the True Programmer: ", ConsoleColor.Gray);

Game game = new Game(playerName, gameplayMode);
game.PlayGame();


GameplayMode SelectGameplayMode()
{
    ColoredConsole.WriteLine("""
    This game can be played in the following ways:
        1 - Human vs. Computer
        2 - Human vs. Human
        3 - Computer vs. Computer
    """, ConsoleColor.Gray);

    while (true)
    {
        string input = ColoredConsole.PromptUser("Make your selection: ", ConsoleColor.Gray);

        if (int.TryParse(input, out int choice))
        {
            if (choice >= 1 && choice <= 3) return (GameplayMode)choice;
        }

        ColoredConsole.WriteLine("Invalid input. Please enter one of the available choices (1-3).", ConsoleColor.DarkRed);
    }
}


// Defines the different gameplay modes the player can choose for a match of the game
public enum GameplayMode { HumanVsComputer = 1, HumanVsHuman = 2, ComputerVsComputer = 3 }
