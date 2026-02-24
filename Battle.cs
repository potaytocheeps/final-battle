/// <summary>
/// A battle consists of several turns where each character in each party takes an action.
/// The battle is over once one of the two parties has been defeated.
/// </summary>
public class Battle
{
    private readonly Player _player1;
    private readonly Player _player2;
    private bool _battleIsOver;
    public static int RoundNumber { get; private set; }

    public Battle(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _battleIsOver = false;
        RoundNumber = 1;
    }

    public void Play()
    {
        Console.Clear();

        while (!_battleIsOver)
        {
            PlayRound();
            RoundNumber++;
        }

        // After the battle, the winning player loots the losing player's party for any unused items and unequipped gear
        if (_player1.Party.Characters.Count > 0)
        {
            _player1.Loot(_player2.Party);
            RemoveWinningPartyStatusEffects(_player1.Party);
        }
        else
        {
            _player2.Loot(_player1.Party);
            RemoveWinningPartyStatusEffects(_player2.Party);
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
            Console.Clear();

            if (currentPlayer.PlayerNumber == 1)
            {
                ConsoleIOHandler.DisplayBattleStatus
                (
                    player1Party: currentPlayer.Party,
                    player2Party: enemyPlayer.Party,
                    character,
                    RoundNumber
                );
            }
            else
            {
                ConsoleIOHandler.DisplayBattleStatus
                (
                    player1Party: enemyPlayer.Party,
                    player2Party: currentPlayer.Party,
                    character,
                    RoundNumber
                );
            }

            ColoredConsole.WriteLine($"Player {currentPlayer.PlayerNumber}");
            ColoredConsole.WriteLine($"It is {character}'s turn...");

            // If this character is electrified, they will forfeit their turn
            if (character.StatusEffects.ContainsKey(StatusEffectType.Electrified))
            {
                character.StatusEffects[StatusEffectType.Electrified].Resolve(character);
                ConsoleIOHandler.WaitForPlayerConfirmation();
                continue;
            }

            currentPlayer.TakeTurn(character, enemyPlayer, RoundNumber);

            // Allow player to see the results of the turn and continue when they are ready
            ConsoleIOHandler.WaitForPlayerConfirmation();

            foreach (StatusEffect statusEffect in character.StatusEffects.Values)
            {
                statusEffect.Resolve(character);
                ConsoleIOHandler.WaitForPlayerConfirmation();
            }

            // Check if character died from a status effect
            if (character.CurrentHP <= 0)
            {
                // Opposing player loots character's gear, if they had any equipped
                if (character.HasGearEquipped)
                {
                    enemyPlayer.LootEnemyCharacter(character);
                    ConsoleIOHandler.WaitForPlayerConfirmation();
                }

                currentPlayer.Party.RemoveFromParty(character);
            }

            // Check to see if either player's party has been completely defeated
            if (_player1.Party.Characters.Count == 0 || _player2.Party.Characters.Count == 0)
            {
                _battleIsOver = true;
                return;
            }
        }
    }

    private void RemoveWinningPartyStatusEffects(Party winningParty)
    {
        foreach (Character character in winningParty.Characters)
        {
            character.RemoveAllStatusEffects();
        }
    }
}
