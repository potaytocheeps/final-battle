PlayerMode playerMode;
Difficulty gameDifficulty;

while (true)
{
    ConsoleIOHandler.DisplayTitle();
    playerMode = ConsoleIOHandler.SelectPlayerMode();
    bool difficultyWasSelected = ConsoleIOHandler.TrySelectDifficulty(out gameDifficulty);

    if (difficultyWasSelected) break;

    Console.Clear();
}

ColoredConsole.WriteLine("");
string playerName = ColoredConsole.PromptUser("Enter the name for the True Programmer: ", ConsoleColor.Gray);

Game game = new Game(playerName, playerMode, gameDifficulty);
game.PlayGame();


// Defines the different gameplay modes the player can choose for a match of the game
public enum PlayerMode { HumanVsComputer, HumanVsHuman, ComputerVsComputer }
