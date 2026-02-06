/// <summary>
/// The basic attack for each character.
/// </summary>
public class StandardAttack : Attack
{
    public StandardAttack(string name) : base(name, AttackType.Standard) { }

    public override int CalculateDamage(Attack attack)
    {
        Damage = attack switch
        {
            BoneCrunch => new Random().Next(2),
            Punch      => 1,
            Unraveling => new Random().Next(3),
            _          => 0
        };

        return Damage;
    }
}
