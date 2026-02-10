/// <summary>
/// A special attack that can be gained based on equipped gear.
/// </summary>
public class SpecialAttack : Attack
{
    public SpecialAttack(string name) : base(name, AttackType.Special) { }

    public override int CalculateDamage()
    {
        Damage = this switch
        {
            Slash     => 2,
            Stab      => 1,
            QuickShot => 3,
            _         => 0
        };

        return Damage;
    }
}
