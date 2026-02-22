/// <summary>
/// A status effect that deals fire damage over time based on a percentage of
/// the inflicted character's max HP.
/// </summary>
public class Burned : StatusEffect
{
    protected override int Damage { get; set; }
    public override DamageType DamageType => DamageType.Fire;
    public override StatusEffectType StatusEffectType => StatusEffectType.Burned;

    public Burned(int attackTargetMaxHP) : base("Burn", numberOfTurns: 3)
    {
        // Burn damage will be 10% of the target's max HP, or at least 1 damage
        Damage = Math.Max(attackTargetMaxHP / 10, 1);
    }

    public override void Resolve(Character target)
    {
        base.Resolve(target);

        string damageType = DamageType.ToString().ToUpper();

        ColoredConsole.WriteLine($"{this} dealt {TextColor.ColorText($"{Damage} {damageType}", DamageType)} damage to {target}.");
        target.TakeDamage(Damage);
    }
}
