/// <summary>
/// A status effect that deals poison damage over time to the character to
/// whom it was applied.
/// </summary>
public class Poisoned : StatusEffect
{
    public override int Damage { get; protected set; }
    public override DamageType DamageType => DamageType.Poison;
    public override StatusEffectType StatusEffectType => StatusEffectType.Poisoned;

    public Poisoned(int damageDealt) : base("Poison")
    {
        Damage = damageDealt;
    }

    public override void Resolve(Character target)
    {
        if (target is MylaraAndSkorin) ColoredConsole.Write($"{target} are ");
        else ColoredConsole.Write($"{target} is ");

        ColoredConsole.WriteLine($"{TextColor.ColorText(StatusEffectName, DamageType)}.");

        string damageType = DamageType.ToString().ToUpper();

        ColoredConsole.WriteLine($"{this} dealt {TextColor.ColorText($"{Damage} {damageType}", DamageType)} damage to {target}.");
        target.TakeDamage(Damage);

        Damage--;
        if (Damage <= 0) target.RemoveStatusEffect(this);
    }
}
