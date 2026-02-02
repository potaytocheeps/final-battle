/// <summary>
/// A Player in the game that controls a Party of Characters.
/// </summary>
public abstract class Player
{
    public Party Party { get; private set; }
    protected readonly int _playerNumber;
    private static int _instanceCount = 0; // Used to keep track of who is player 1 and who is player 2

    public Player(Party party)
    {
        Party = party;

        _instanceCount++;
        if (_instanceCount > 2) _instanceCount = 1; // There should only be two instances in existence during a match

        _playerNumber = _instanceCount;
    }

    public void SetParty(Party party)
    {
        Party = party;
    }

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound);
    protected abstract ActionType SelectAction();
    public abstract (AttackType, Character) PerformAttack(Character currentCharacter, Party enemyParty);
    protected abstract AttackType SelectAttack(Character currentCharacter);
    protected abstract Character SelectAttackTarget(Party enemyParty);
    public abstract Item SelectItem();
}
