/// <summary>
/// Contains the main logic for the whole game.
/// </summary>
public class Game
{
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly List<Party> _enemyBattleParties;
    private Battle _battle;

    public Game(string name, PlayerMode playerMode, Difficulty gameDifficulty)
    {
        _enemyBattleParties = PartyGenerator.GenerateParties(gameDifficulty);

        switch (playerMode)
        {
            case PlayerMode.HumanVsComputer:
                _player1 = new HumanPlayer(GetHeroParty(name), playerNumber: 1);
                _player2 = new ComputerPlayer(_enemyBattleParties[0], playerNumber: 2); // Start with the first enemy party
                break;
            case PlayerMode.HumanVsHuman:
                _player1 = new HumanPlayer(GetHeroParty(name), playerNumber: 1);
                _player2 = new HumanPlayer(_enemyBattleParties[0], playerNumber: 2);
                break;
            default:
                _player1 = new ComputerPlayer(GetHeroParty(name), playerNumber: 1);
                _player2 = new ComputerPlayer(_enemyBattleParties[0], playerNumber: 2);
                break;
        }

        _battle = new Battle(_player1, _player2);
    }

    public void PlayGame()
    {
        Console.Clear();
        int currentBattleIndex = 0;
        bool isFinalBattle;

        while (true)
        {
            Console.Clear();

            isFinalBattle = currentBattleIndex == _enemyBattleParties.Count - 1;

            // Display current battle information
            ConsoleIOHandler.DisplayBattleInfo(_player1, _player2, battleNumber: currentBattleIndex + 1, isFinalBattle);
            ConsoleIOHandler.WaitForPlayerConfirmation();

            _battle.Play();

            // End game if hero party was defeated during this current battle
            if (_player1.Party.Characters.Count == 0)
            {
                ColoredConsole.WriteLine("The heroes lost! The Uncoded One's forces have prevailed.", ConsoleColor.DarkRed);
                return;
            }

            currentBattleIndex++;

            if (currentBattleIndex >= _enemyBattleParties.Count) break; // End game if there are no more battles left
            else
            {
                // Continue to next battle
                _player2.SetParty(_enemyBattleParties[currentBattleIndex]);
                _battle = new Battle(_player1, _player2);
            }
        }

        // Hero party emerged victorious from all battles
        ColoredConsole.WriteLine("The heroes won! The Uncoded One's reign is finally over!", ConsoleColor.Green);
    }

    private Party GetHeroParty(string name)
    {
        return new Party
        (
            characters: [new TrueProgrammer(name), new VinFletcher(), new MylaraAndSkorin()],
            startingItems: [..HealthPotion.CreatePotions(numberOfSmall: 2, numberOfMedium: 1, numberOfLarge: 1),
                            ..CurePotion.CreatePotions(numberOfBurn: 1, numberOfElectrified: 1, numberOfPoison: 1)],
            startingGear: []
        );
    }
}


public enum Difficulty { Easy, Medium, Hard }
