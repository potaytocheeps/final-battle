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

        // After the battle, the winning player loots the losing player's party for any unused items and unequipped gear
        if (_player1.Party.Characters.Count > 0) _player1.Loot(_player2.Party);
        else _player2.Loot(_player1.Party);
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
            if (currentPlayer.PlayerNumber == 1)
            {
                ConsoleIOHandler.DisplayBattleStatus
                (
                    player1Party: currentPlayer.Party,
                    player2Party: enemyPlayer.Party,
                    character,
                    _roundNumber
                );
            }
            else
            {
                ConsoleIOHandler.DisplayBattleStatus
                (
                    player1Party: enemyPlayer.Party,
                    player2Party: currentPlayer.Party,
                    character,
                    _roundNumber
                );
            }

            ColoredConsole.WriteLine($"Player {currentPlayer.PlayerNumber}");
            ColoredConsole.WriteLine($"It is {character}'s turn...");

            currentPlayer.TakeTurn(character, enemyPlayer, _roundNumber);

            // Allow player to see the results of the turn and continue when they are ready
            ConsoleIOHandler.WaitForPlayerConfirmation();

            // Check to see if either player's party has been completely defeated
            if (_player1.Party.Characters.Count == 0 || _player2.Party.Characters.Count == 0)
            {
                _battleIsOver = true;
                return;
            }
        }
    }
}
