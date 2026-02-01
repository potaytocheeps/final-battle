/// <summary>
/// A computer-controlled player.
/// This player has basic AI logic that will allow it to take pre-determined
/// actions for the different choices that a human player would otherwise
/// need to make.
/// </summary>
public class ComputerPlayer : Player
{
    public ComputerPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer)
    {
        Thread.Sleep(500);
        ActionType action = SelectAction();
        currentCharacter.PerformAction(currentPlayer: this, action, enemyPlayer);
    }

    protected override ActionType SelectAction()
    {
        return ActionType.Attack;
    }

    public override (AttackType, Character) PerformAttack(Character _, Party enemyParty)
    {
        Character attackTarget = SelectAttackTarget(enemyParty);
        AttackType attackType = SelectAttack(_);

        return (attackType, attackTarget);
    }

    protected override AttackType SelectAttack(Character _)
    {
        return AttackType.Standard;
    }

    protected override Character SelectAttackTarget(Party enemyParty)
    {
        int enemyPartySize = enemyParty.Characters.Count;
        int randomIndex = new Random().Next(enemyPartySize);
        return enemyParty.Characters[randomIndex];
    }
}
