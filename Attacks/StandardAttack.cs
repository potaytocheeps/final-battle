/// <summary>
/// The basic attack for each character.
/// </summary>
public class StandardAttack : Attack
{
    public StandardAttack(string name, DamageType damageType = DamageType.Normal) : base(name, AttackType.Standard, damageType) { }

    public override int CalculateDamage()
    {
        Damage = this switch
        {
            Punch or Bite => 1,
            BoneCrunch    => Random.Shared.Next(2),
            Unraveling    => Random.Shared.Next(5),
            _             => 0
        };

        return Damage;
    }
}
