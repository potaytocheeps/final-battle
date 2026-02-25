/// <summary>
/// The basic attack for each character.
/// </summary>
public class StandardAttack : Attack
{
    public StandardAttack(string name, DamageType damageType = DamageType.Physical) : base(name, AttackType.Standard, damageType) { }

    public override int CalculateDamage()
    {
        Damage = this switch
        {
            Punch         => 1,
            Bite          => 2,
            BoneCrunch    => Random.Shared.Next(1, 4),
            Unraveling    => Random.Shared.Next(4, 7),
            _             => 0
        };

        return Damage;
    }
}
