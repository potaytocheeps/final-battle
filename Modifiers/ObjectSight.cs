/// <summary>
/// Defensive attack modifier that gives a character resistance to Decoding
/// type damage by providing a 2-point damage reduction against Decoding
/// attacks.
/// </summary>
public class ObjectSight: Modifier
{
    public ObjectSight() : base("Object Sight", damageReductionAmount: 2, ModifierType.Defensive) { }

    public override int CalculateModifiedDamage(int damage, DamageType attackDamageType)
    {
        if (attackDamageType == DamageType.Decoding)
        {
            return base.CalculateModifiedDamage(damage, attackDamageType);
        }

        return damage;
    }
}
