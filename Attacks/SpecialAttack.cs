/// <summary>
/// A special attack that can be gained based on equipped gear.
/// </summary>
public class SpecialAttack : Attack
{
    public SpecialAttack(string name, DamageType damageType = DamageType.Normal, bool givesStatusEffect = false)
                  : base(name, AttackType.Special, damageType, givesStatusEffect) { }

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
