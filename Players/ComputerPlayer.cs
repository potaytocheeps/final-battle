/// <summary>
/// A computer-controlled player.
/// This player has basic AI logic that will allow it to take pre-determined
/// actions for the different choices that a human player would otherwise
/// need to make.
/// </summary>
public class ComputerPlayer : Player
{
    public ComputerPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound)
    {
        if (_playerNumber == 1) Battle.DisplayBattleStatus(player1Party: Party, player2Party: enemyPlayer.Party, currentCharacter, currentRound);
        else Battle.DisplayBattleStatus(player1Party: enemyPlayer.Party, player2Party: Party, currentCharacter, currentRound);

        ColoredConsole.WriteLine($"Player {_playerNumber}");
        ColoredConsole.WriteLine($"It is {currentCharacter}'s turn...");
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

    public override Item SelectItem()
    {
        throw new NotImplementedException();
    }
}
