/// <summary>
/// The basic attack for each character.
/// </summary>
public class StandardAttack : Attack
{
    public StandardAttack(string name) : base(name) { }

    public override int CalculateDamage(Character character)
    {
        Damage = character switch
        {
            Skeleton       => new Random().Next(2),
            TrueProgrammer => 1,
            TheUncodedOne  => new Random().Next(3),
            _              => 0
        };

        return Damage;
    }
}
