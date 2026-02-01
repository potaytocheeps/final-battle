/// <summary>
/// The most basic enemy character in the game.
/// </summary>
public class Skeleton : Character
{
    public Skeleton() : base("Skeleton", maxHP: 5)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Bone Crunch"));
    }

    public Skeleton(int number) : base($"Skeleton {number}", maxHP: 5)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Bone Crunch"));
    }
}
