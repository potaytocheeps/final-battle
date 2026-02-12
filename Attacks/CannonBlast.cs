/// <summary>
/// Special attack provided by the Cannon of Consolas gear item.
/// Its attack damage depends on the current round number for that battle.
/// </summary>
public class CannonBlast : SpecialAttack
{
    public CannonBlast() : base("Cannon Blast") { }

    public override int CalculateDamage()
    {
        bool turnIsMultipleOfThree = Battle.RoundNumber % 3 == 0;
        bool turnIsMultipleOfFive = Battle.RoundNumber % 5 == 0;

        if (turnIsMultipleOfThree && turnIsMultipleOfFive) return 10;
        else if (turnIsMultipleOfThree || turnIsMultipleOfFive) return 3;
        else return 1;
    }
}
