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
        List<Character> playerParty = [new TrueProgrammer(name), new VinFletcher(), new MylaraAndSkorin()];
        List<Item> heroPartyItems = [new SmallHealthPotion(), new SmallHealthPotion(), new SmallHealthPotion(), new SimulasSoup(), new SimulasSoup()];
        List<Item> enemyPartyItems = [new SmallHealthPotion()];
        Party defaultHeroParty = new Party
        (
            characters: playerParty,
            startingItems: heroPartyItems,
            startingGear: []
        );

        _enemyBattleParties =
            [
                new Party // Battle 1
                (
                    characters: [RandomCharacterGenerator.GetRandomEnemy(Difficulty.Easy),
                                 RandomCharacterGenerator.GetRandomEnemy(Difficulty.Easy),
                                 RandomCharacterGenerator.GetRandomEnemy(Difficulty.Easy)],
                    startingItems: enemyPartyItems,
                    startingGear: [new Sword(), new Dagger()]
                ),
                new Party // Battle 2
                (
                    characters: [RandomCharacterGenerator.GetRandomEnemy(Difficulty.Medium),
                                 RandomCharacterGenerator.GetRandomEnemy(Difficulty.Easy),
                                 RandomCharacterGenerator.GetRandomEnemy(Difficulty.Medium)],
                    startingItems: enemyPartyItems,
                    startingGear: [new Dagger(), new BinaryHelm()]
                ),
                new Party // Battle 3
                (
                    characters: [RandomCharacterGenerator.GetRandomEnemy(Difficulty.Medium),
                                 RandomCharacterGenerator.GetRandomEnemy(Difficulty.Medium),
                                 RandomCharacterGenerator.GetRandomEnemy(Difficulty.Hard)],
                    startingItems: enemyPartyItems,
                    startingGear: []
                ),
                new Party // Final Battle
                (
                    characters: [new TheUncodedOne()],
                    startingItems: [new SimulasSoup()],
                    startingGear: new ()
                )
            ];

        switch (gameplayMode)
        {
            case GameplayMode.HumanVsComputer:
                _player1 = new HumanPlayer(defaultHeroParty);
                _player2 = new ComputerPlayer(_enemyBattleParties[0]); // Start with the first enemy party
                break;
            case GameplayMode.HumanVsHuman:
                _player1 = new HumanPlayer(defaultHeroParty);
                _player2 = new HumanPlayer(_enemyBattleParties[0]);
                break;
            default:
                _player1 = new ComputerPlayer(defaultHeroParty);
                _player2 = new ComputerPlayer(_enemyBattleParties[0]);
                break;
        }

        _battle = new Battle(_player1, _player2);
    }

    public void PlayGame()
    {
        Console.Clear();
        int currentBattleIndex = 0;

        while (true)
        {
            // Display current battle information
            ConsoleIOHandler.DisplayBattleInfo(_player1, _player2, battleNumber: currentBattleIndex + 1);
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
}


public enum Difficulty { Easy, Medium, Hard }
