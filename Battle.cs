/// <summary>
/// A battle consists of several turns where each character in each party takes an action.
/// The battle is over once one of the two parties has been defeated.
/// </summary>
public class Battle
{
    private readonly Player _player1;
    private readonly Player _player2;
    private bool _battleIsOver;

    public Battle(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _battleIsOver = false;
    }

    public void Play()
    {
        while (!_battleIsOver)
        {
            PlayRound();
        }
    }

    public void PlayRound()
    {
        Console.WriteLine("Player 1");
        TakePlayerTurn(currentPlayer: _player1, enemyPlayer: _player2);

        if (_battleIsOver) return;

        Console.WriteLine("Player 2");
        TakePlayerTurn(currentPlayer: _player2, enemyPlayer: _player1);
    }

    private void TakePlayerTurn(Player currentPlayer, Player enemyPlayer)
    {
        foreach (Character character in currentPlayer.Party.Characters)
        {
            Console.WriteLine($"It is {character.Name}'s turn...");
            currentPlayer.TakeTurn(character, enemyPlayer);
            Console.WriteLine();

            // Check to see if either player's party has been completely defeated
            if (_player1.Party.Characters.Count == 0 || _player2.Party.Characters.Count == 0)
            {
                _battleIsOver = true;
                return;
            }
        }
    }
}
