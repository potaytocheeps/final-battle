/// <summary>
/// Special attack provided by the Dagger gear item.
/// </summary>
public class Stab : SpecialAttack
{
    public Stab() : base("Stab", DamageType.Poison, givesStatusEffect: true) { }
}
