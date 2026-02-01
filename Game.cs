/// <summary>
/// Contains the main logic for the whole game.
/// </summary>
public class Game
{
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly List<Party> _enemyBattleParties;
    private Battle _battle;

    public Game(string name, GameplayMode gameplayMode)
    {
        _enemyBattleParties =
            [
                new Party(new Skeleton()), // Battle 1
                new Party(new Skeleton(1), new Skeleton(2)), // Battle 2
                new Party(new TheUncodedOne()) // Battle 3
            ];

        // Initialize variables to prevent warning of potential null values
        _player1 = new ComputerPlayer(new Party(new TrueProgrammer(name)));
        _player2 = new ComputerPlayer(new Party(new TheUncodedOne()));

        switch (gameplayMode)
        {
            case GameplayMode.HumanVsComputer:
                _player1 = new HumanPlayer(new Party(new TrueProgrammer(name)));
                _player2 = new ComputerPlayer(_enemyBattleParties[0]); // Start with the first enemy party
                break;
            case GameplayMode.HumanVsHuman:
                _player1 = new HumanPlayer(new Party(new TrueProgrammer(name)));
                _player2 = new HumanPlayer(_enemyBattleParties[0]);
                break;
            case GameplayMode.ComputerVsComputer:
                _player1 = new ComputerPlayer(new Party(new TrueProgrammer(name)));
                _player2 = new ComputerPlayer(_enemyBattleParties[0]);
                break;
        }

        _battle = new Battle(player1: _player1, player2: _player2);
    }

    public void PlayGame()
    {
        int currentBattleIndex = 0;

        while (true)
        {
            // Display current battle information
            Console.WriteLine("---------------------------------------------");
            Console.Write($"Battle {currentBattleIndex + 1}: ");
            DisplayPlayerParty(_player1);
            Console.Write(" vs. ");
            DisplayPlayerParty(_player2);
            Console.WriteLine("\n---------------------------------------------\n");

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
                _battle = new Battle(player1: _player1, player2: _player2);
            }
        }

        // Hero party emerged victorious from all battles
        ColoredConsole.WriteLine("The heroes won! The Uncoded One's reign is finally over!", ConsoleColor.Green);
    }

    private void DisplayPlayerParty(Player player)
    {
        int playerPartySize = player.Party.Characters.Count;

        for (int index = 0; index < playerPartySize; index++)
        {
            string characterName = player.Party.Characters[index].Name;

            if (index >= playerPartySize - 1) Console.Write(characterName); // This is the last character in the party
            else Console.Write(characterName + ", ");
        }
    }
}
