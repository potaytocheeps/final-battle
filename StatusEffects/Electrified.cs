/// <summary>
/// A status effect that prevents the affected character from acting during
/// their turn.
/// </summary>
public class Electrified : StatusEffect
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Electrified;
    protected override DamageType DamageType => DamageType.Electric;

    public Electrified() : base(numberOfTurns: 1) { }

    public override void Resolve(Character target)
    {
        base.Resolve(target);

        ColoredConsole.WriteLine($"{target} CAN'T MOVE!");
    }

    public override string ToString() => Name.ToUpper();
}
