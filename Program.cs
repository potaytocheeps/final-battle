ConsoleIOHandler.DisplayTitle();
PlayerMode playerMode = ConsoleIOHandler.SelectPlayerMode();
Console.WriteLine();
string playerName = ColoredConsole.PromptUser("Enter the name for the True Programmer: ", ConsoleColor.Gray);

Game game = new Game(playerName, playerMode);
game.PlayGame();


// Defines the different gameplay modes the player can choose for a match of the game
public enum PlayerMode { HumanVsComputer = 1, HumanVsHuman = 2, ComputerVsComputer = 3 }
