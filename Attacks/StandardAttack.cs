/// <summary>
/// The basic attack for each character.
/// </summary>
public class StandardAttack : Attack
{
    public StandardAttack(string name) : base(name, AttackType.Standard) { }

    public override int CalculateDamage()
    {
        Damage = this switch
        {
            Punch or Bite => 1,
            BoneCrunch    => Random.Shared.Next(2),
            Unraveling    => Random.Shared.Next(3),
            _             => 0
        };

        return Damage;
    }
}
