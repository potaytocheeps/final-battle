/// <summary>
/// A special attack that can be gained based on equipped gear.
/// </summary>
public class SpecialAttack : Attack
{
    public SpecialAttack(string name) : base(name, AttackType.Special) { }

    public override int CalculateDamage(Attack attack)
    {
        Damage = attack switch
        {
            Slash => 2,
            Stab  => 1,
            _     => 0
        };

        return Damage;
    }
}
