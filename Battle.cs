/// <summary>
/// A battle consists of several turns where each character in each party takes an action.
/// The battle is over once one of the two parties has been defeated.
/// </summary>
public class Battle
{
    private readonly Player _player1;
    private readonly Player _player2;
    private bool _battleIsOver;
    private int _roundNumber;

    public Battle(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _battleIsOver = false;
        _roundNumber = 1;
    }

    public void Play()
    {
        while (!_battleIsOver)
        {
            PlayRound();
            _roundNumber++;
        }
    }

    public void PlayRound()
    {
        TakePlayerTurn(currentPlayer: _player1, enemyPlayer: _player2);

        if (_battleIsOver) return;

        TakePlayerTurn(currentPlayer: _player2, enemyPlayer: _player1);
    }

    private void TakePlayerTurn(Player currentPlayer, Player enemyPlayer)
    {
        foreach (Character character in currentPlayer.Party.Characters)
        {
            currentPlayer.TakeTurn(character, enemyPlayer, _roundNumber);
            Console.WriteLine();

            // Check to see if either player's party has been completely defeated
            if (_player1.Party.Characters.Count == 0 || _player2.Party.Characters.Count == 0)
            {
                _battleIsOver = true;
                return;
            }
        }
    }

    public static void DisplayBattleStatus(Party player1Party, Party player2Party, Character currentCharacter, int currentRound)
    {
        ColoredConsole.WriteLine($"===================== ROUND {currentRound} =============================\n");
        player1Party.DisplayPartyInfo(currentCharacter);
        ColoredConsole.WriteLine("\n------------------------ vs -------------------------------\n");
        player2Party.DisplayPartyInfo(currentCharacter, addPadding: true);
        ColoredConsole.WriteLine("\n===========================================================");
    }
}
