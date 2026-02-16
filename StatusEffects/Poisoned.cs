/// <summary>
/// A status effect that deals poison damage over time to the character to
/// whom it was applied.
/// </summary>
public class Poisoned : StatusEffect
{
    protected override string Name { get; }
    protected override int Damage { get; }
    protected override DamageType DamageType => DamageType.Poison;
    public override StatusEffectType StatusEffectType => StatusEffectType.Poisoned;

    public Poisoned() : base(numberOfTurns: 3)
    {
        Name = "Poison";
        Damage = 1;
    }

    public override void Resolve(Character target)
    {
        base.Resolve(target);

        string damageType = DamageType.ToString().ToUpper();

        ColoredConsole.WriteLine($"{this} dealt {Damage} {damageType} damage to {target}.");
        target.TakeDamage(Damage);
    }

    public override string ToString() => Name.ToUpper();
}
