/// <summary>
/// A Player in the game that controls a Party of Characters.
/// </summary>
public abstract class Player
{
    public Party Party { get; private set; }

    public Player(Party party)
    {
        Party = party;
    }

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer);
    protected abstract ActionType SelectAction();
    public abstract (AttackType, Character) PerformAttack(Character currentCharacter, Party enemyParty);
    protected abstract AttackType SelectAttack(Character currentCharacter);
    protected abstract Character SelectAttackTarget(Party enemyParty);

    public void SetParty(Party party)
    {
        Party = party;
    }
}
