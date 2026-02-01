GameplayMode gameplayMode = SelectGameplayMode();
Console.WriteLine();
string playerName = AskUserForInput("Enter the name for the True Programmer: ");

Game game = new Game(playerName, gameplayMode);
game.PlayGame();


string AskUserForInput(string question)
{
    while (true)
    {
        Console.Write(question);
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input)) continue;
        else return input;
    }
}


GameplayMode SelectGameplayMode()
{
    Console.WriteLine("""
    This game can be played in the following ways:
        1 - Human vs. Computer
        2 - Human vs. Human
        3 - Computer vs. Computer
    """);

    while (true)
    {
        string input = AskUserForInput("Make your selection: ");

        if (int.TryParse(input, out int choice))
        {
            if (choice >= 1 && choice <= 3) return (GameplayMode)choice;
        }

        Console.WriteLine("Invalid input. Please enter one of the available choices (1-3).");
    }
}


// Defines the different gameplay modes the player can choose for a match of the game
public enum GameplayMode { HumanVsComputer = 1, HumanVsHuman = 2, ComputerVsComputer = 3 }
